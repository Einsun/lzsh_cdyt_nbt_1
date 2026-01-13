
using System.Collections.Concurrent;

namespace EasyTCP
{
    /// <summary>
    /// 
    /// </summary>
    public class Channels
    {
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentDictionary<string, Channel> OpenChannels;
        private readonly Server thisServer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myServer"></param>
        public Channels(Server myServer)
        {
            OpenChannels = new ConcurrentDictionary<string, Channel>();
            thisServer = myServer;
        }
    }
}
