using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using Serilog.Events;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AeDataService
{
    internal class MqttService : IHostedService
    {
        DataHelper dataHelper = new DataHelper();
        IMqttClient Client { get; set; }
        System.Timers.Timer TraderTimer = new System.Timers.Timer(1000);
        string Server { get; set; }
        string FilePath { get; set; }
        int Port { get; set; }
        private ILogger Logger { get; set; }

        public MqttService(IConfiguration config)
        {
            string SerilogOutputTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);
            Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug() // 捕获的最小日志级别
                        .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\logs\\log.txt", rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, retainedFileCountLimit: null)
                        .CreateLogger();
            var constr = config.GetConnectionString("DataContext");
            dataHelper.ConStr = constr??"";
            Server = config.GetSection("MqttServer").Value ?? "127.0.0.1";
            FilePath = config.GetSection("FilePath").Value ?? "";
            Port = config.GetValue<int>("MqttPort");
            Logger.Information($"{constr}-{Server}-{Port}-{FilePath}");
            var factory = new MqttFactory();
            Client = factory.CreateMqttClient();
            Client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
            Client.DisconnectedAsync += Client_DisconnectedAsync;
            TraderTimer.Elapsed += TraderTimer_Elapsed;
        }

        bool flag = false;
        bool reconnect = false;
        AeWave wave = new AeWave();
        private void TraderTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!flag)
            {
                flag = true;
                try
                {
                    if (!Client.IsConnected)
                    {
                        if (!reconnect)
                        {
                            reconnect = true;
                            _ = Client.ReconnectAsync();
                        }
                    }
                    else if (wave.DBId == 0 || (DateTime.Now - wave.Time).TotalSeconds > 120)
                    {
                        var data = dataHelper.GetData();
                        if (data != null && data.DBId > 0)
                        {
                            wave.DBId = data.DBId;
                            wave.SN = data.SN;
                            wave.DeviceId = data.DeviceId;
                            wave.Path = data.Path;
                            _ = SendMessageAsync(wave);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, ex.Message);
                }

                flag = false;
            }
        }

        private async Task SendMessageAsync(AeWave wave)
        {
            MqttApplicationMessage m = new MqttApplicationMessage();
            m.Topic = $"PUB/Cmd/{wave.SN}";
            wave.ID = (wave.ID + 1) % 50000;
            var data = new
            {
                MessageId = wave.ID,
                CmdType = "TXCollectData",
                Data = new
                {
                    CollectDataAddr = Path.Combine(FilePath, wave.DeviceId.ToString(), wave.Path),
                },
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
            };
            wave.Time = DateTime.Now;
            Console.WriteLine(JsonSerializer.Serialize(data));
            m.PayloadSegment = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            await Client.PublishAsync(m);
        }

        async Task StartMqtt()
        {
            await Client.TryDisconnectAsync();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(Server, Port)
                .Build();

            try
            {
                await Client.ConnectAsync(options);
                await Client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("PUB/Fault/#").Build());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }
        }

        private Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            reconnect = false;
            return Task.CompletedTask;
        }

        Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                Logger.Information(arg.ApplicationMessage.Topic);
                Console.WriteLine(arg.ApplicationMessage.Topic);
                var data = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
                Logger.Information(data);
                Console.WriteLine(data);
                var json = JsonNode.Parse(data);
                if (json?["MessageId"]?.GetValue<int>() == wave.ID)
                {
                    var _data = json["Data"];
                    if (_data != null)
                    {
                        dataHelper.Update(wave.DBId, _data);
                        wave.DBId = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        public void Stop()
        {
            Client.DisconnectAsync();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //var data = "{\"Data\": {\"RootMeanSquare\": 7.022738633543037e-05, \"FaultSta\": 0, \"FaultEnum\": null, \"MesDataimage1\": \"D:\\\\python\\\\PyCharm Community Edition 2023.2.1\\\\Project\\\\Time_frequency_analyze\\\\MQTT_test\\\\44mm_time_frequency_graph_1705162830.7248676.tif\"}, \"Time\": \"2024-01-14 00:20:31.199\"}";
            //var json = JsonNode.Parse(data);
            //if (json?["MessageId"]?.GetValue<int>() == wave.ID)
            //{
            //    dataHelper.Update(wave.DBId, json["Data"]);
            //}
            await StartMqtt();
            TraderTimer.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TraderTimer.Stop();
            return Task.CompletedTask;
        }
    }
}
