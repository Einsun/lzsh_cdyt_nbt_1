
using System;
using System.Linq;

namespace EasyTCP
{
    /// <summary>
    /// 
    /// </summary>
    public class DataReceivedArgs : EventArgs, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public byte Addr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Channel ThisChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RemoteIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if( ThisChannel != null ) ThisChannel.Dispose();
        }
    }
}
