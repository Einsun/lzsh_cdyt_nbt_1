using NBWebApp.Models;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace NBWebApp.Utils
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisHelper : IDisposable
    {
        //连接字符串
        private string _connectionString;
        //实例名称
        private string _instanceName;
        //默认数据库
        private int _defaultDB;
        /// <summary>
        /// 
        /// </summary>
        public List<AeDevInfo> AeDevInfos=new List<AeDevInfo>();

        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        /// <param name="defaultDB"></param>
        public RedisHelper(string connectionString, string instanceName, int defaultDB = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDB = defaultDB;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        /// <summary>
        /// 获取ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        /// <summary>
        /// 获取redis数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase(_defaultDB);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="endPointsIndex"></param>
        /// <returns></returns>
        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }

}
