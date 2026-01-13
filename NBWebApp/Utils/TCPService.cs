#pragma warning disable    1591

using EasyTCP;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Controllers;
using NBWebApp.Models;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
namespace NBWebApp.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class TCPService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<TCPService> _logger;
        readonly List<DeviceInfo> devInfos = new List<DeviceInfo>();
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_scopeFactory"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public TCPService(IServiceScopeFactory _scopeFactory, ILogger<TCPService> logger, IConfiguration config)
        {
            this.scopeFactory = _scopeFactory;
            _logger = logger;
            _config=config;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                Server server = new Server("0.0.0.0", 9093);
                string savepath =_config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo di = new DirectoryInfo(savepath);
                var dr = System.IO.DriveInfo.GetDrives();
                foreach (var d in dr)
                {
                    if (d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable)
                    {
                        if (d.RootDirectory.Name == di.Root.Name)
                        {
                            if (d.TotalFreeSpace < 1024 * 1024 * 1024)
                            {
                                _logger.LogWarning("{}-{}", d.TotalFreeSpace, d.AvailableFreeSpace);
                            }
                            else
                            {
                                _logger.LogInformation("{}-{}", d.TotalFreeSpace, d.AvailableFreeSpace);
                            }
                        }
                    }
                }
                server.DataReceived += server_OnDataIn;
                server.Start();
            });

            return Task.CompletedTask;
        }

        void server_OnDataIn(object sender, DataReceivedArgs e)
        {
            try
            {
                _logger.LogWarning("{}-{}",e.Addr,e.SN);
                if (e.Type < 2)
                {
                    _logger.LogInformation("8880-{}-{}-{}", e.RemoteIP, e.Type, e.SN);
                    var dev = devInfos.Find(t => t.ConnectID == e.ConnectionId);
                    if (dev != null)
                    {
                        dev.State = 1;
                        if (e.Type == 1)
                        {
                            if (dev.TimingLength > 0 && dev.TimingSleep > 0)
                            {
                                if ((DateTime.Now - dev.Starttime).TotalSeconds > dev.TimingLength)
                                {
                                    if ((DateTime.Now - dev.Starttime).TotalSeconds > dev.TimingSleep)
                                    {
                                        if (dev.AeWaveData == null)
                                        {
                                            dev.AeWaveData = new WaveData();
                                        }
                                        else
                                        {
                                            if (!dev.Save)
                                            {
                                                dev.Save = true;
                                                SaveAeData(dev);
                                            }
                                            dev.AeWaveData.data.Clear();
                                            dev.AeWaveData.StartTime = DateTime.Now;
                                        }
                                        var _data = e.Data;
                                        if (_data != null)
                                        {
                                            var a = BitConverter.ToUInt32(_data, 0);
                                            var b = BitConverter.ToUInt32(_data, 4);
                                            string ver = Encoding.UTF8.GetString(_data, 8, 4);
                                            var c = BitConverter.ToUInt32(_data, 12);
                                            var d = BitConverter.ToUInt32(_data, 16);
                                            var ee = BitConverter.ToUInt32(_data, 20);
                                            var f = BitConverter.ToDouble(_data, 24);
                                            Int16[] wave = new short[c];
                                            Buffer.BlockCopy(_data, 32, wave, 0, (int)c * 2);
                                            dev.AeWaveData.unixtime = a;
                                            dev.AeWaveData.points = c;
                                            dev.AeWaveData.speed = d;
                                            dev.AeWaveData.gain = ee;
                                            dev.AeWaveData.enlarge = f;
                                            dev.AeWaveData.data.AddRange(wave);
                                            dev.AeWaveData.EndTime = DateTime.Now;
                                            dev.Starttime = DateTime.Now;
                                            dev.Save = false;
                                        }

                                    }
                                    else if (!dev.Save)
                                    {
                                        dev.Save = true;
                                        var _data = e.Data;
                                        if (_data != null)
                                        {
                                            var a = BitConverter.ToUInt32(_data, 0);
                                            var b = BitConverter.ToUInt32(_data, 4);
                                            string ver = Encoding.UTF8.GetString(_data, 8, 4);
                                            var c = BitConverter.ToUInt32(_data, 12);
                                            var d = BitConverter.ToUInt32(_data, 16);
                                            var ee = BitConverter.ToUInt32(_data, 20);
                                            var f = BitConverter.ToDouble(_data, 24);
                                            Int16[] wave = new short[c];
                                            Buffer.BlockCopy(_data, 32, wave, 0, (int)c * 2);
                                            if (dev.AeWaveData != null)
                                            {
                                                dev.AeWaveData.EndTime = DateTime.Now;
                                                dev.AeWaveData.data.AddRange(wave);
                                                SaveAeData(dev);
                                                dev.AeWaveData.data.Clear();
                                                dev.AeWaveData.StartTime = DateTime.Now;
                                            }
                                        }

                                    }
                                    else if (dev.AeWaveData != null)
                                    {
                                        dev.AeWaveData.data.Clear();
                                        dev.AeWaveData.StartTime = DateTime.Now;
                                        var _data = e.Data;
                                        if (_data != null)
                                        {
                                            var a = BitConverter.ToUInt32(_data, 0);
                                            var b = BitConverter.ToUInt32(_data, 4);
                                            string ver = Encoding.UTF8.GetString(_data, 8, 4);
                                            var c = BitConverter.ToUInt32(_data, 12);
                                            var d = BitConverter.ToUInt32(_data, 16);
                                            var ee = BitConverter.ToUInt32(_data, 20);
                                            var f = BitConverter.ToDouble(_data, 24);
                                            Int16[] wave = new short[c];
                                            Buffer.BlockCopy(_data, 32, wave, 0, (int)c * 2);
                                            dev.AeWaveData.EndTime = DateTime.Now;
                                            dev.AeWaveData.data.AddRange(wave);
                                        }

                                    }
                                }
                                else
                                {
                                    var _data = e.Data;
                                    if (_data != null)
                                    {
                                        var a = BitConverter.ToUInt32(_data, 0);
                                        var b = BitConverter.ToUInt32(_data, 4);
                                        string ver = Encoding.UTF8.GetString(_data, 8, 4);
                                        var c = BitConverter.ToUInt32(_data, 12);
                                        var d = BitConverter.ToUInt32(_data, 16);
                                        var ee = BitConverter.ToUInt32(_data, 20);
                                        var f = BitConverter.ToDouble(_data, 24);
                                        Int16[] wave = new short[c];
                                        Buffer.BlockCopy(_data, 32, wave, 0, (int)c * 2);
                                        dev.AeWaveData.EndTime = DateTime.Now;
                                        dev.AeWaveData?.data.AddRange(wave);
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogInformation("TimingLength-{}-TimingSleep-{}", dev.TimingLength, dev.TimingSleep);
                            }
                        }
                        else if (e.Type == 0)
                        {
                            var _data = e.Data;
                            if (_data != null)
                            {
                                string ver = Encoding.UTF8.GetString(_data, 0, 4);
                                var a = BitConverter.ToUInt32(_data, 4);
                                var b = BitConverter.ToUInt32(_data, 8);
                                var c = BitConverter.ToDouble(_data, 12);
                                var d = BitConverter.ToDouble(_data, 20);
                                var ee = BitConverter.ToDouble(_data, 28);
                                var f = BitConverter.ToDouble(_data, 36);
                                var g = BitConverter.ToUInt32(_data, 44);
                                var h = BitConverter.ToUInt32(_data, 48);
                                var i = BitConverter.ToUInt32(_data, 52);
                                var j = BitConverter.ToUInt32(_data, 56);
                                //dev.Data = data;
                                string sql =
                               $"""
        INSERT INTO `nbdata`.`aedatas`
        (`Sn`,`DeviceId`,`UnixTime`,`AMP`,`Power`,`RMS`,`ASL`,`RisingTime`,`RisingNum`,`DuringTime`,`RingingNum`,`InsertTime`)
        VALUES('{dev.Sn}',{dev.DeviceId},{a}.{b},{c},{d},{ee},{f},{g},{h},{i},{j},'{DateTime.Now:yyyy-MM-dd HH:mm:ss}');
        """;
                                dev = new DeviceInfo();
                                using (var scope = scopeFactory.CreateScope())
                                {
                                    var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();

                                    _context.Database.ExecuteSqlRaw(sql);
                                }
                            }
                        }
                    }
                    else
                    {
                        dev = new DeviceInfo();
                        if (e.SN != null)
                        {
                            using (var scope = scopeFactory.CreateScope())
                            {
                                var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                                var devs = _context.Devices.Where(t => t.SN != null && t.SN.EndsWith(e.SN)).FirstOrDefault();
                                if (devs != null)
                                {
                                    dev.Sn = devs.SN;
                                    dev.State = 1;
                                    dev.ConnectID = e.ConnectionId;
                                    dev.DeviceId = devs.Id;
                                    dev.ProductLineId = devs.Id;
                                    devs.State = 1;
                                    _context.SaveChanges();
                                    var aerule = _context.AeRules.Find(devs.AeRuleId);
                                    if (aerule != null)
                                    {
                                        var node = JsonNode.Parse(aerule.TimingConfig);
                                        dev.TimingLength = node?["ae_timing_length"]?.GetValue<int>() ?? 0;
                                        dev.TimingSleep = node?["ae_timing_sleep"]?.GetValue<int>() ?? 0;
                                    }
                                    if (e.Type == 1)
                                    {
                                        dev.Starttime = DateTime.Now;
                                        var _data = e.Data;
                                        if (_data != null)
                                        {
                                            var a = BitConverter.ToUInt32(_data, 0);
                                            var b = BitConverter.ToUInt32(_data, 4);
                                            string ver = Encoding.UTF8.GetString(_data, 8, 4);
                                            var c = BitConverter.ToUInt32(_data, 12);
                                            var d = BitConverter.ToUInt32(_data, 16);
                                            var ee = BitConverter.ToUInt32(_data, 20);
                                            var f = BitConverter.ToDouble(_data, 24);
                                            Int16[] wave = new short[c];
                                            Buffer.BlockCopy(_data, 32, wave, 0, (int)c * 2);
                                            //dev.Data = $"{c}-{_data.Length}";
                                            dev.AeWaveData = new WaveData();
                                            dev.AeWaveData.unixtime = a;
                                            dev.AeWaveData.points = c;
                                            dev.AeWaveData.speed = d;
                                            dev.AeWaveData.gain = ee;
                                            dev.AeWaveData.enlarge = f;
                                            dev.AeWaveData.StartTime = DateTime.Now;
                                            dev.AeWaveData.data.AddRange(wave);
                                        }
                                    }
                                    else if (e.Type == 0)
                                    {
                                        var _data = e.Data;
                                        if (_data != null)
                                        {
                                            string ver = Encoding.UTF8.GetString(_data, 0, 4);
                                            var a = BitConverter.ToUInt32(_data, 4);
                                            var b = BitConverter.ToUInt32(_data, 8);
                                            var c = BitConverter.ToDouble(_data, 12);
                                            var d = BitConverter.ToDouble(_data, 20);
                                            var ee = BitConverter.ToDouble(_data, 28);
                                            var f = BitConverter.ToDouble(_data, 36);
                                            var g = BitConverter.ToUInt32(_data, 44);
                                            var h = BitConverter.ToUInt32(_data, 48);
                                            var i = BitConverter.ToUInt32(_data, 52);
                                            var j = BitConverter.ToUInt32(_data, 56);
                                            //dev.Data = data;
                                            string sql =
                                           $"""
        INSERT INTO `nbdata`.`aedatas`
        (`Sn`,`DeviceId`,`UnixTime`,`AMP`,`Power`,`RMS`,`ASL`,`RisingTime`,`RisingNum`,`DuringTime`,`RingingNum`,`InsertTime`)
        VALUES('{dev.Sn}',{dev.DeviceId},{a}.{b},{c},{d},{ee},{f},{g},{h},{i},{j},'{DateTime.Now:yyyy-MM-dd HH:mm:ss}');
        """;
                                            _context.Database.ExecuteSqlRaw(sql);
                                        }
                                    }
                                    devInfos.Add(dev);
                                }
                            }

                        }
                    }
                }
                else
                {
                    _logger.LogInformation("8880-{}-{}-{}", e.RemoteIP, e.Type,e.SN);
                    var dev = devInfos.Find(t => t.ConnectID == e.ConnectionId);
                    if (dev != null)
                    {
                        dev.State = e.Type == 2 ? 1 : 0;
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                            var _dev = _context.Devices.Find(dev.DeviceId);
                            if(_dev != null)
                            {
                                _dev.State = dev.State;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
            }
        }

        void SaveAeData(DeviceInfo dev)
        {
            string savepath = _config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;

            savepath = Path.Combine(savepath, dev.DeviceId.ToString());
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            DirectoryInfo di= new DirectoryInfo(savepath);
            var dr = System.IO.DriveInfo.GetDrives();
            foreach(var d in dr)
            {
                if(d.DriveType==DriveType.Fixed||d.DriveType==DriveType.Removable)
                {
                    if (d.RootDirectory.Name == di.Root.Name)
                    {
                        if (d.TotalFreeSpace<1024*1024*1024)
                        {
                            Console.WriteLine(d.TotalFreeSpace);
                            return;
                        }
                    }
                }
            }

            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                Models.AeWaveData devData = new Models.AeWaveData();
                devData.DeviceId = dev.DeviceId;
                devData.SN = dev.Sn ?? "";
                devData.Points = (int)dev.AeWaveData.data.Count;
                devData.Enlarge = dev.AeWaveData.enlarge;
                devData.Speed = (int)dev.AeWaveData.speed;
                devData.Gain = (int)dev.AeWaveData.gain;
                devData.StartTime = dev.AeWaveData.StartTime;
                devData.EndTime = dev.AeWaveData.EndTime;
                var data = dev.AeWaveData.data.Select(t => (((t / 30000.0f) * 2.048) / devData.Enlarge / 10 / 10).ToString("F6"));
                var filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".data";
                File.WriteAllText(Path.Combine(savepath, filename), string.Join(',',data));
                devData.Data = filename;
                _context.Add(devData);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProductLineId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Sn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public WaveData AeWaveData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConnectID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TimingLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TimingSleep { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Save { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WaveData
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 points { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 speed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 gain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double enlarge { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UInt32 unixtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Int16> data = new List<short>();
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.Now;
    }
}
