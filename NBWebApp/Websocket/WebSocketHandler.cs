using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

namespace NBWebApp.Websocket
{
    /// <summary>
    ///  WebSocket 抽象类
    /// </summary>
    public abstract class WebSocketHandler
    {
        /// <summary>
        /// 
        /// </summary>
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSocketConnectionManager"></param>
        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        /// <summary>
        /// 根据 stocketId获取对应的WebSocket
        /// </summary>
        /// <param name="socketId"></param>
        /// <returns></returns>
        public virtual  WebSocket GetWebStocket(string socketId)
        {
            return WebSocketConnectionManager.GetSocketById(socketId);
        }

        /// <summary>
        /// 连接一个 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual void OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
        }

        /// <summary>
        /// 连接一个 socket （指定socketId）
        /// </summary>
        /// <param name="socketId"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual void OnConnected(string socketId, WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socketId, socket);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            if (!string.IsNullOrWhiteSpace(socketId))
                await WebSocketConnectionManager.RemoveSocket(socketId).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送消息给指定 socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            var encodedMessage = Encoding.UTF8.GetBytes(message);
            try
            {
                await socket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage,
                                                                      offset: 0,
                                                                      count: encodedMessage.Length),
                                       messageType: WebSocketMessageType.Text,
                                       endOfMessage: true,
                                       cancellationToken: CancellationToken.None).ConfigureAwait(false);
            }
            catch (WebSocketException e)
            {
                if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    await OnDisconnected(socket);
                }
            }
        }
        /// <summary>
        /// 发送消息给指定id的socket
        /// </summary>
        /// <param name="socketId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string socketId, string message)
        {
            var socket = WebSocketConnectionManager.GetSocketById(socketId);
            if (socket != null)
                await SendMessageAsync(socket, message).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送消息给多个指定id的socket
        /// </summary>
        /// <param name="sockets"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(List<string> sockets, string message)
        {
            foreach (var socket in sockets)
            {
                await SendMessageAsync(socket, message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 获取所有 socket 连接
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return WebSocketConnectionManager.GetAll();
        }

        /// <summary>
        /// 给所有 socket 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                try
                {
                    if (pair.Value.State == WebSocketState.Open)
                        await SendMessageAsync(pair.Value, message).ConfigureAwait(false);
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        await OnDisconnected(pair.Value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToGroupAsync(string groupID, string message)
        {
            var sockets = WebSocketConnectionManager.GetAllFromGroup(groupID);
            if (sockets != null)
            {
                foreach (var socket in sockets)
                {
                    await SendMessageAsync(socket, message);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="message"></param>
        /// <param name="except"></param>
        /// <returns></returns>
        public async Task SendMessageToGroupAsync(string groupID, string message, string except)
        {
            var sockets = WebSocketConnectionManager.GetAllFromGroup(groupID);
            if (sockets != null)
            {
                foreach (var id in sockets)
                {
                    if (id != except)
                        await SendMessageAsync(id, message);
                }
            }
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="receivedMessage"></param>
        /// <returns></returns>
        public virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string receivedMessage)
        {
            try
            {
                await Task.CompletedTask;
               // await SendMessageAsync(socket, receivedMessage).ConfigureAwait(false);
            }
            catch (TargetParameterCountException)
            {
                //await SendMessageAsync(socket, new Message() { }).ConfigureAwait(false);
            }
            catch (ArgumentException)
            {
                //await SendMessageAsync(socket, new Message() { }).ConfigureAwait(false);
            }
        }
    }
}
