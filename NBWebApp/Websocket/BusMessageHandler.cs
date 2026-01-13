using System.Net.WebSockets;

namespace NBWebApp.Websocket
{
    /// <summary>
    /// 业务逻辑处理
    /// </summary>
    public class BusMessageHandler : WebSocketHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSocketConnectionManager"></param>
        public BusMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="receivedMessage"></param>
        /// <returns></returns>
        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string receivedMessage)
        {
            return base.ReceiveAsync(socket, result, receivedMessage);
        }

    
    }
}
