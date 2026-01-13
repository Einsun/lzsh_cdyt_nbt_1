
using Microsoft.EntityFrameworkCore;
using MQTTnet.Client;
using MQTTnet;
using NBWebApp.Controllers;
using NBWebApp.Models;
using QcMessages;
using Google.Protobuf;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using NBWebApp.Websocket;

namespace NBWebApp.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class AeConfigService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly RedisHelper _redis;
        private readonly ILogger<AeConfigService> _logger;
        private Timer _timer;
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_scopeFactory"></param>
        /// <param name="logger"></param>
        /// <param name="redisHelper"></param>
        /// <param name="config"></param>
        public AeConfigService(IServiceScopeFactory _scopeFactory, 
            ILogger<AeConfigService> logger, RedisHelper redisHelper, IConfiguration config)
        {
            this.scopeFactory = _scopeFactory;
            _logger = logger;
            _redis = redisHelper;
            _config = config;
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
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
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
        /// <summary>  
        /// 将c# 本地DateTime时间格式转换为Unix时间戳格式 （毫秒）
        /// </summary>  
        /// <param name="localTime">时间</param>  
        /// <returns>long</returns>  
        public long ConvertLocalDateTimeToUtcTimestamp(DateTime localTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime);
            DateTime utcStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan ts = utcTime - utcStartTime;
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        private void DoWork(object state)
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                  //var webs=  scope.ServiceProvider.GetService<BusMessageHandler>();
                  //  webs.SendMessageToAllAsync(new Message() { userName = "test" });
                    var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                    string v = "";
                    do
                    {
                        v = _redis.GetDatabase().ListRightPop("device").ToString();
                        if (!string.IsNullOrEmpty(v))
                        {
                            _logger.LogInformation(v);
                            int id = Convert.ToInt32(v);
                            var server = _config.GetSection("MQTTServer").Value;
                            var topic = _config.GetSection("MQTTTopic").Value;
                            if (string.IsNullOrEmpty(server))
                                server = "127.0.0.1";
                            if (string.IsNullOrEmpty(topic))
                                topic = "qciot/ae/new/pub/";
                            var factory = new MqttFactory();
                            var client = factory.CreateMqttClient();
                            var build = new MqttClientOptionsBuilder()
                                .WithTcpServer(server, 1883);
                            if (!string.IsNullOrEmpty(_config.GetSection("MQTTUser").Value))
                                build = build.WithCredentials(_config.GetSection("MQTTUser").Value,
                                    _config.GetSection("MQTTPassord").Value
                                    );
                            var options = build
                                .Build();
                            var dev = _context.Devices.Find(id);
                            if (dev != null)
                            {
                                var rule = _context.AeRules.Find(dev.AeRuleId);
                                if (rule != null)
                                {

                                    List<byte[]> data = new List<byte[]>();
                                    var jsona = JsonNode.Parse(rule.MeasureConfig);
                                    if (jsona != null)
                                    {
                                        SensorMessage msg = new SensorMessage();
                                        msg.PacketName = "RAEM1";
                                        msg.NodeId = dev.SN;
                                        msg.GatewayId = "0001";
                                        msg.Ver = "V1.0.96_20240110";
                                        msg.UnixTime = (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                        msg.Microsecond = (int)(((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - msg.UnixTime) * 1000000);
                                        msg.AeMeasureConfig = new AeMeasureConfig();
                                        msg.AeMeasureConfig.AeThreshold = jsona["ae_threshold"]?.GetValue<double>() ?? 30;
                                        msg.AeMeasureConfig.AeMeasureSpeed = jsona["ae_measure_speed"]?.GetValue<uint>() ?? 800;
                                        msg.AeMeasureConfig.AeMeasureMode = jsona["ae_measure_mode"]?.GetValue<uint>() ?? 1;
                                        msg.AeMeasureConfig.AeEet = jsona["ae_eet"]?.GetValue<uint>() ?? 12500;
                                        msg.AeMeasureConfig.AeHdt = jsona["ae_hdt"]?.GetValue<uint>() ?? 1250;
                                        msg.AeMeasureConfig.AeHlt = jsona["ae_hlt"]?.GetValue<uint>() ?? 18;
                                        msg.AeMeasureConfig.AeMeasureLength = jsona["ae_measure_length"]?.GetValue<uint>() ?? 1500;
                                        msg.AeMeasureConfig.AeMeasureTimes = jsona["ae_measure_times"]?.GetValue<uint>() ?? 20000;
                                        msg.AeMeasureConfig.AeMeasureInterval = jsona["ae_measure_interval"]?.GetValue<uint>() ?? 1000;
                                        msg.AeMeasureConfig.AeParaEnable = jsona["ae_para_enable"]?.GetValue<bool>() ?? false;
                                        msg.AeMeasureConfig.AeWaveEnable = jsona["ae_wave_enable"]?.GetValue<bool>() ?? false;
                                        msg.AeMeasureConfig.AeSystemTime = msg.UnixTime;
                                        msg.AeMeasureConfig.AeMeasureState = jsona["ae_measure_state"]?.GetValue<bool>() ?? false;
                                        _logger.LogInformation(JsonSerializer.Serialize(msg));
                                        data.Add(msg.ToByteArray());
                                    }
                                    //var jsonb = JsonNode.Parse(rule.LevelConfig);
                                    //if (jsonb != null)
                                    //{
                                    //    SensorMessage msg = new SensorMessage();
                                    //    msg.PacketName = "RAEM1";
                                    //    msg.NodeId = dev.SN;
                                    //    msg.GatewayId = "0001";
                                    //    msg.Ver = "V2.0";
                                    //    msg.UnixTime = 946687610;
                                    //    msg.Microsecond = 146967;
                                    //    msg.AeTimingConfig = new AeTimingConfig();
                                    //    msg.AeTimingConfig.AeTimingEnable = true;
                                    //    msg.AeTimingConfig.AeTimingType = 3;
                                    //    msg.AeTimingConfig.AeTimingLength = json["ae_timing_length"]?.GetValue<uint>() ?? 5;
                                    //    msg.AeTimingConfig.AeTimingSleep = json["ae_timing_sleep"]?.GetValue<uint>() ?? 3600;
                                    //    data.Add(msg.ToByteArray());
                                    //}
                                    var json = JsonNode.Parse(rule.TimingConfig);
                                    if (json != null)
                                    {
                                        SensorMessage msg = new SensorMessage();
                                        msg.PacketName = "RAEM1";
                                        msg.NodeId = dev.SN;
                                        msg.GatewayId = "0001";
                                        msg.Ver = "V1.0.96_20240110";
                                        msg.UnixTime = (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                        msg.Microsecond = (int)(((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - msg.UnixTime) * 1000000);
                                        msg.AeTimingConfig = new AeTimingConfig();
                                        msg.AeTimingConfig.AeTimingEnable = true;
                                        msg.AeTimingConfig.AeTimingType = 3;
                                        msg.AeTimingConfig.AeTimingLength = json["ae_timing_length"]?.GetValue<uint>() ?? 5;
                                        msg.AeTimingConfig.AeTimingSleep = json["ae_timing_sleep"]?.GetValue<uint>() ?? 3600;
                                        _logger.LogInformation(JsonSerializer.Serialize(msg));
                                        data.Add(msg.ToByteArray());
                                    }

                                    data.Add(Encoding.UTF8.GetBytes("qciot_system_reboot"));
                                    Task.Factory.StartNew(async () =>
                                    {
                                        await client.ConnectAsync(options);

                                        foreach (var d in data)
                                        {
                                            MqttApplicationMessage m = new MqttApplicationMessage();
                                            m.Topic = topic + dev.SN;
                                            m.PayloadSegment = d;
                                            await client.PublishAsync(m);
                                            Thread.Sleep(500);
                                        }
                                    });

                                }
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(v));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
            }
        }
    }
}
