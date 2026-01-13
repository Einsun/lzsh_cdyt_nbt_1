using log4net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace EasyTCP
{
    /// <summary>
    /// 
    /// </summary>
    public class Channel : IDisposable
    {
        private readonly Server thisServer;
        /// <summary>
        /// 
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// 
        /// </summary>
        public readonly int DataType;
        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; } = "";
        List<byte[]> CMD = new List<byte[]>();
        List<byte> Address= new List<byte>();
        private TcpClient thisClient;
        private readonly byte[] buffer;
        byte[] sndata=new byte[4];
        private NetworkStream stream;
        private bool isOpen;
        private bool disposed;
        string RemoteIP = "";
        ILog log;
        DateTime? StartTime = null;
        DateTime? EndTime = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myServer"></param>
        /// <param name="type"></param>
        public Channel(Server myServer,int type=0)
        {
            thisServer = myServer;
            buffer = new byte[16];
            DataType = type;
            Id = Guid.NewGuid().ToString();
            log = LogManager.GetLogger(this.GetType());
            if (type > 0)
            {
                TradeTimer.Elapsed += TradeTimer_Elapsed;
                TradeTimer.Start();
            }
        }
        /// <summary>
        /// 发送一次
        /// </summary>
        /// <param name="sn"></param>
        public void SendOne(string sn)
        {
            if (sn == this.SN)
            {
                foreach (var data in CMD)
                {
                    if (stream.CanWrite)
                    {
                        List<byte> _data = new List<byte>();
                        _data.AddRange(sndata);
                        _data.AddRange(data);
                        stream.Write(_data.ToArray(), 0, _data.Count);
                        //log.Info(BitConverter.ToString(_data.ToArray()));
                        // stream.Write(sndata, 0, sndata.Length);
                        // stream.Write(data, 0, data.Length);
                        Thread.Sleep(100);
                    }
                }
            }
        }
        private void TradeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SN) && stream != null)
                {
                    if (StartTime != null && StartTime < DateTime.Now)
                    {
                        return;
                    }
                    else if (EndTime != null && EndTime > DateTime.Now)
                    {
                        return;
                    }
                    foreach (var data in CMD)
                    {
                        if (stream.CanWrite)
                        {
                            List<byte> _data = new List<byte>();
                            _data.AddRange(sndata);
                            _data.AddRange(data);
                            stream.Write(_data.ToArray(), 0, _data.Count);
                            //log.Info(BitConverter.ToString(_data.ToArray()));
                            // stream.Write(sndata, 0, sndata.Length);
                            // stream.Write(data, 0, data.Length);
                            Thread.Sleep(100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TradeTimer.Stop();
            }
        }

        System.Timers.Timer TradeTimer = new System.Timers.Timer(1000);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Open(TcpClient client)
        {
            thisClient = client;
            isOpen = true;
            if (thisServer.ConnectedChannels != null && !thisServer.ConnectedChannels.OpenChannels.TryAdd(Id, this))
            {
                isOpen = false;
                throw (new Exception("Unable to add channel to channel list"));
            }
            RemoteIP = client.Client.RemoteEndPoint.ToString();
            thisServer.OnDataIn(new DataReceivedArgs()
            {
                Message = "",
                ConnectionId = Id,
                Type = 2,
                RemoteIP = RemoteIP,
                ThisChannel = this
            });

            using (stream = thisClient.GetStream())
            {
                int position;

                while (isOpen)
                {
                    try
                    {
                        if (ClientDisconnected())
                        {
                            Close();
                        }
                        else if (DataType == 0)
                        {
                            while ((position = stream.Read(buffer, 0, buffer.Length)) == 16 && isOpen)
                            {
                                if (buffer[0] == 0xa5 && buffer[1] == 0xa5 && buffer[2] == 0xa5 && buffer[3] == 0xa5)
                                {
                                    string sn = Encoding.UTF8.GetString(buffer, 4, 4);
                                    int type = BitConverter.ToInt32(buffer, 8);
                                    int len = BitConverter.ToInt32(buffer, 12);
                                    var _data = new byte[len];
                                    int index = 0;
                                    do
                                    {
                                        position = stream.Read(_data, index, len - index);
                                        index += position;
                                    }
                                    while (index < len);

                                    var args = new DataReceivedArgs()
                                    {
                                        Data = _data,
                                        Type = type,
                                        SN = sn,
                                        RemoteIP = RemoteIP,
                                        ConnectionId = Id,
                                        ThisChannel = this
                                    };
                                    //var arr = new List<byte>();
                                    //arr.AddRange(buffer);
                                    //arr.AddRange(_data);
                                    //File.WriteAllBytes("log/"+DateTime.Now.ToString("yyyyMMddHHmmss.ffff")+".bin", arr.ToArray());
                                    thisServer.OnDataIn(args);

                                }
                                else if(buffer[0] == 0x47 && buffer[1] == 0x45 && buffer[2] == 0x54 && buffer[3] == 0x20){
                                    Console.WriteLine(Encoding.UTF8.GetString(buffer,0,position));
                                    Close();
                                }
                                else
                                {
                                    Console.WriteLine(BitConverter.ToString(buffer, 0, position));
                                }
                                if (!isOpen) { break; }
                            }
                        }
                        else
                        {
                            byte[] data = new byte[50];
                            position = stream.Read(data, 0, 25);
                            if (position > 0)
                            {
                                //log.Info(BitConverter.ToString(data, 0, position));
                                //Console.WriteLine(BitConverter.ToString(data, 0, position));
                                if (position == 4)
                                {
                                    if (string.IsNullOrEmpty(SN))
                                    {
                                        SN = BitConverter.ToString(data, 0, position).Replace("-", "");
                                        Array.Copy(data, 0, sndata, 0, 4);

                                        var args = new DataReceivedArgs()
                                        {
                                            Data = null,
                                            RemoteIP = RemoteIP,
                                            Type = 4,
                                            SN = this.SN,
                                            ConnectionId = Id,
                                            ThisChannel = this
                                        };
                                        thisServer.OnDataIn(args);
                                    }
                                }
                                else if (position == 25 && !string.IsNullOrEmpty(SN))
                                {
                                    var type = 4;
                                    if (data[4] == Address[Address.Count - 1])
                                    {
                                        type = 5;
                                    }
                                    var args = new DataReceivedArgs()
                                    {
                                        Data = data.AsSpan(7, 16).ToArray(),
                                        Type = type,
                                        Addr = data[4],
                                        SN = this.SN,
                                        ConnectionId = Id,
                                        RemoteIP = RemoteIP,
                                        ThisChannel = this
                                    };
                                    thisServer.OnDataIn(args);
                                }
                            }
                            if (!isOpen) { break; }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        Close();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            stream?.Write(data, 0, data.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            stream?.Write(data, 0, data.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] CRC16(byte[] data)
        {
            int len = data.Length-2;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置
                data[len] = lo;
                data[len + 1] = hi;
                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addrs"></param>
        /// <param name="interval"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        public void SetAddress(List<int> addrs,int interval=1,DateTime? starttime=null,DateTime? endtime=null)
        {
            TradeTimer.Interval = interval * 1000;
            StartTime = starttime;
            EndTime = endtime;
            foreach (var addr in addrs)
            {
                byte ad =(byte)addr;
                byte[] cmd = new byte[] { 0x01, 0x04, 0x01, 0x00, 0x00, 0x08, 0x00, 0x00 };
                cmd[0]= ad;
                CRC16(cmd);
                CMD.Add(cmd);
                Address.Add(ad);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            TradeTimer.Stop();
            Dispose(false);
            isOpen = false;
            thisServer.ConnectedChannels?.OpenChannels.TryRemove(Id, out _);
            var args = new DataReceivedArgs()
            {
                Message = "",
                ConnectionId = Id,
                Type = 3,
                RemoteIP = RemoteIP,
                ThisChannel = this
            };

            thisServer.OnDataIn(args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                stream?.Close();
                thisClient?.Close();
                disposed = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool ClientDisconnected()
        {
            return (thisClient?.Client.Available == 0 && thisClient.Client.Poll(1, SelectMode.SelectRead));
        }
    }
}
