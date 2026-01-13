using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace NBWebApp.Websocket
{
    /// <summary>
    ///  WebSocket 管理
    /// </summary>
    public class WebSocketConnectionManager
    {
        /// <summary>
        /// 用户连接池
        /// </summary>
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private ConcurrentDictionary<string, List<string>> _groups = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 获取指定id的socket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WebSocket GetSocketById(string id)
        {
            if (_sockets.TryGetValue(id, out WebSocket socket))
                return socket;
            else
                return null;
        }

        /// <summary>
        /// 获取所有socket
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        /// <summary>
        /// 根据 socket 获取其id
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        /// <summary>
        /// 添加socket连接
        /// </summary>
        /// <param name="socket"></param>
        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        /// <summary>
        /// 添加指定id的socket连接
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="socket"></param>
        public void AddSocket(string socketID, WebSocket socket)
        {
            _sockets.TryAdd(socketID, socket);
        }

        /// <summary>
        /// 删除指定 id 的 socket，并关闭连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveSocket(string id)
        {
            if (id == null) return;

            if (_sockets.TryRemove(id, out WebSocket socket))
            {
                if (socket.State != WebSocketState.Open) return;

                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the WebSocketManager",
                                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 创建 socket 的 id
        /// </summary>
        /// <returns></returns>
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取socket连接总数量
        /// </summary>
        /// <returns></returns>
        public int GetSocketClientCount()
        {
            return _sockets.Count();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<string> GetAllFromGroup(string GroupID)
        {
            if (_groups.ContainsKey(GroupID))
            {
                return _groups[GroupID];
            }

            return default(List<string>);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="groupID"></param>
        public void AddToGroup(string socketID, string groupID)
        {
            if (_groups.ContainsKey(groupID))
            {
                _groups[groupID].Add(socketID);

                return;
            }

            _groups.TryAdd(groupID, new List<string> { socketID });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="groupID"></param>
        public void RemoveFromGroup(string socketID, string groupID)
        {
            if (_groups.ContainsKey(groupID))
            {
                _groups[groupID].Remove(socketID);
            }
        }
    }
}
