using System.Net.WebSockets;
using System.Text;

namespace NBWebApp.Websocket
{
    /// <summary>
    /// 自定义 WebSocket 中间件
    /// </summary>
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }
        private readonly ILogger<WebSocketManagerMiddleware> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="webSocketHandler"></param>
        /// <param name="logger"></param>
        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler,
                                          ILogger<WebSocketManagerMiddleware> logger
                                          )
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            //接受 websocket 客户端连接  // 转换当前连接为一个 ws 连接
            var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);

            string socketId =Guid.NewGuid().ToString();

            _webSocketHandler.OnConnected(socketId, socket);

            await Receive(socket, async (result, serializedMessage) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    try
                    {
                        _logger.LogInformation("{}", serializedMessage);
                        await _webSocketHandler.ReceiveAsync(socket, result, serializedMessage).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("{}", ex);
                    }
                    return;
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    try
                    {
                        await _webSocketHandler.OnDisconnected(socket);
                    }
                    catch (WebSocketException)
                    {
                        throw; //let's not swallow any exception for now
                    }
                    return;
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            // 判断连接类型，并执行相应操作
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                string message = null;
                WebSocketReceiveResult result = null;
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            // 继续接受信息
                            result = await socket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            message = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }

                    handleMessage(result, message);
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        socket.Abort();
                    }
                }
            }

            await _webSocketHandler.OnDisconnected(socket);
        }

    }
}
