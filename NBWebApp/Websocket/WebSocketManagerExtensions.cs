using System.Reflection;

namespace NBWebApp.Websocket
{
    /// <summary>
    /// 中间件扩展方法
    /// </summary>
    public static class WebSocketManagerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services, Assembly assembly = null)
        {
            services.AddTransient<WebSocketConnectionManager>();

            Assembly ass = assembly ?? Assembly.GetEntryAssembly();

            foreach (var type in ass.ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
        /// <summary>
        /// 通过扩展方法公开中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                              PathString path,
                                                              WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
    }
}
