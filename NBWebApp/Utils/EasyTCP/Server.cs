using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EasyTCP
{
  /// <summary>
  /// 
  /// </summary>
    public class Server
    {
        private bool _running;
        /// <summary>
        /// 
        /// </summary>
        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                _running = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DataReceivedArgs> DataReceived;
        private readonly TcpListener Listener;
        /// <summary>
        /// 
        /// </summary>
        public Channels ConnectedChannels;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public Server(string ip,int port)
        {
            Listener = new TcpListener(IPAddress.Parse(ip), port);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start(int type=0)
        {
            try
            {
                Listener.Start();
                Running = true;
                ConnectedChannels = new Channels(this);
                while (Running)
                {
                    var client= Listener.AcceptTcpClient();
                    Task.Factory.StartNew(() =>
                    {
                        new Channel(this,type).Open(client);
                    });
                }

            }
            catch(SocketException)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Listener.Stop();
            Running = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnDataIn(DataReceivedArgs e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}
