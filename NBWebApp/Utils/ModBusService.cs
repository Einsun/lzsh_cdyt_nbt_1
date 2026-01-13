using EasyTCP;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;
using NBWebApp.Websocket;
using System.Text.Json;
using System.Text.Unicode;
namespace NBWebApp.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class ModBusService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly BusMessageHandler websocketHandler;
        private readonly ILogger<TCPService> _logger;
        List<SignalType> SignalTypes { get; set; } = new List<SignalType>();
        readonly List<ConDevice> devInfos = new List<ConDevice>();
        JsonSerializerOptions jsonoption = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All) };
        Server Server { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_scopeFactory"></param>
        /// <param name="logger"></param>
        public ModBusService(IServiceScopeFactory _scopeFactory, ILogger<TCPService> logger)
        {
            this.scopeFactory = _scopeFactory;
            _logger = logger;
            using (var scope = scopeFactory.CreateScope())
            {
                websocketHandler = scope.ServiceProvider.GetService<BusMessageHandler>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        public void SendOne(string sn)
        {
            var data = Server.ConnectedChannels.OpenChannels;
            foreach(var t in data)
            {
                t.Value.SendOne(sn);
            }
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
            Task.Factory.StartNew(() =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                    _context.Database.ExecuteSqlRaw("update devices set state=0");
                }
                Server = new Server("0.0.0.0", 9100);
                Server.DataReceived += server_OnDataInAsync;
                Server.Start(1);
            });

            return Task.CompletedTask;
        }

        async void server_OnDataInAsync(object sender, DataReceivedArgs e)
        {
            try
            {
                if (e.Type >= 4)
                {
                    if (e.Data == null)
                    {
                        var devinfo = devInfos.Find(t => t.SN == e.SN);
                        if (devinfo == null)
                        {
                            devinfo = new ConDevice();
                            devinfo.SN = e.SN ?? "";
                            devInfos.Add(devinfo);
                        }
                        devinfo.ConnectID = e.ConnectionId ?? "";
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                            var devs = _context.CommDevices.Where(t => t.SN == e.SN).FirstOrDefault();
                            SignalTypes = _context.SignalType.ToList();

                            if (devs != null)
                            {
                                devs.State = 1;
                                devinfo.State = 1;
                                devinfo.CommDeviceID = devs.Id;
                                devinfo.ProductLineId = devs.ProductLineId;
                                var sensordev = _context.Devices.Where(t => t.CommDeviceId == devs.Id).ToList();
                                devinfo.SensorDevices.Clear();
                                var interval = 1;
                                int state = 1;
                                DateTime? starttime = null;
                                DateTime? endtime = null;
                                if (devs.GatherRuleId != null)
                                {
                                    var gatherrule = _context.GatherRules.Find(devs.GatherRuleId);
                                    if (gatherrule != null)
                                    {
                                        interval = gatherrule.Interval;
                                        starttime = gatherrule.StartTime;
                                        endtime = gatherrule.EndTime;
                                        if (starttime != null && starttime > DateTime.Now)
                                        {
                                            state = 2;
                                        }
                                        else if(endtime!=null && endtime < DateTime.Now)
                                        {
                                            state = 2;
                                        }
                                    }
                                }
                                foreach (var dev in sensordev)
                                {
                                    dev.State = state;
                                    var sensor = new SensorDevice();
                                    sensor.Device = dev;
                                    sensor.AlarmRule = _context.AlarmRules.Find(dev.AlarmRuleId);
                                    sensor.SensorValue = new SensorValue()
                                    {
                                        Channel = dev.Pos,
                                        Name = dev.Name,
                                    };
                                    devinfo.SensorDevices.Add(sensor);
                                }
                                if (Server.ConnectedChannels.OpenChannels.ContainsKey(e.ConnectionId))
                                {
                                    var addrs = devinfo.SensorDevices.Select(t => t.Device.Address).Distinct().ToList();
                                    _logger.LogInformation(JsonSerializer.Serialize(addrs));
                                    Server.ConnectedChannels.OpenChannels[e.ConnectionId].SetAddress(addrs, interval, starttime, endtime);
                                }
                                _context.SaveChanges();
                            }
#if DEBUG
                            else
                            {
                                // var addrs = new List<int>() { 1 };
                                // Server.ConnectedChannels.OpenChannels[e.ConnectionId].SetAddress(addrs, 100000, null, null);
                            }
#endif
                        }
                    }
                    else
                    {
                        _logger.LogInformation("9100-{}-{}", e.SN, BitConverter.ToString(e.Data));
                        var devinfo = devInfos.Find(t => t.SN == e.SN);
                        if (devinfo != null && devinfo.CommDeviceID > 0)
                        {
                            using (var scope = scopeFactory.CreateScope())
                            {
                                var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                                foreach (var sen in devinfo.SensorDevices)
                                {
                                    if (sen.Device != null && sen.Device.Address == e.Addr)
                                    {
                                        sen.SensorValue.State = "正常";
                                        AlarmData alarmData = new AlarmData();
                                        alarmData.Time = DateTime.Now;
                                        alarmData.DeviceId = sen.Device.Id;
                                        int pos = sen.Device.Pos;
                                        int d = e.Data[2 * pos] * 256 + e.Data[2 * pos + 1];
                                        var max = (decimal)Math.Pow(2, sen.Device.Samping);
                                        if (d > max)
                                        {
                                            alarmData.Value = sen.Device.MaxValue;
                                            alarmData.Reason = "超过上限";
                                            alarmData.AlarmType = 1;
                                            sen.SensorValue.State = "故障";
                                            _context.Add(alarmData);
                                            sen.SensorValue.Value = sen.Device.MaxValue;
                                        }
                                        else
                                        {
                                            var sigtype = SignalTypes.Find(t => t.Id == sen.Device.SignalTypeId);
                                            if (sigtype != null)
                                            {
                                                var s = d * (sigtype.MaxValue - sigtype.MinValue) / max;
                                                var v = s / (sigtype.MaxValue - sigtype.MinValue) * (sen.Device.MaxValue - sen.Device.MinValue) + sen.Device.MinValue;
                                                alarmData.Value = v;
                                                if (sen.AlarmRule != null)
                                                {
                                                    if (sen.AlarmRule.Max < v)
                                                    {
                                                        alarmData.AlarmType = 1;
                                                        alarmData.Reason = "超过上限";
                                                        sen.SensorValue.State = "故障";
                                                        _context.Add(alarmData);
                                                    }
                                                    else if (v < sen.AlarmRule.Min)
                                                    {
                                                        alarmData.AlarmType = 1;
                                                        alarmData.Reason = "低于下限";
                                                        sen.SensorValue.State = "故障";
                                                        _context.Add(alarmData);
                                                    }
                                                    else
                                                    {
                                                        alarmData.AlarmType = 0;
                                                        alarmData.Reason = "正常";
                                                        _context.Add(alarmData);
                                                    }
                                                }
                                                else
                                                {
                                                    alarmData.AlarmType = 0;
                                                    alarmData.Reason = "正常";
                                                    _context.Add(alarmData);
                                                }

                                                sen.SensorValue.Value = v;
                                            }
                                        }
                                        _context.SaveChanges();
                                        if (alarmData.AlarmType > 0)
                                        {
                                            var message = JsonSerializer.Serialize(alarmData);
                                            //_logger.LogInformation("Notice:{}", message);
                                            await websocketHandler.SendMessageToAllAsync(message);
                                        }
                                    }
                                }
                                //if (e.Type == 5)
                                //{
                                //    var sensorValues = devinfo.SensorDevices.Select(t => t.SensorValue).ToList();
                                //    SensorData sensorData = new SensorData();
                                //    sensorData.CommDeviceId = devinfo.CommDeviceID;
                                //    sensorData.Time = DateTime.Now;
                                //    sensorData.Data = JsonSerializer.Serialize(sensorValues, jsonoption);
                                //    _context.Add(sensorData);
                                //    var s = new { devinfo.ProductLineId, devinfo.CommDeviceID, data = sensorData.Data };
                                //    var message = JsonSerializer.Serialize(s);
                                //    _logger.LogInformation("Notice:{}", message);
                                //    await websocketHandler.SendMessageToAllAsync(message);
                                //}
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("9100-{}-{}-{}", e.RemoteIP, e.Type, e.SN);
                    var dev = devInfos.Find(t => t.ConnectID == e.ConnectionId);
                    if (dev != null && dev.CommDeviceID > 0)
                    {
                        dev.State = e.Type == 2 ? 1 : 0;
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<NBDataContext>();
                            var com = _context.CommDevices.Find(dev.CommDeviceID);
                            com.State = dev.State;
                            foreach (var devInfo in dev.SensorDevices)
                            {
                                var _dev = _context.Devices.Find(devInfo.Device.Id);
                                if (_dev != null)
                                {
                                    _dev.State = dev.State;
                                    AlarmData alarmData = new AlarmData();
                                    alarmData.Time = DateTime.Now;
                                    alarmData.DeviceId = _dev.Id;
                                    alarmData.Value = 0;
                                    alarmData.AlarmType = 2;
                                    alarmData.Reason = "离线";
                                    _context.Add(alarmData);
                                }
                            }
                            _context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
            }
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
    }

    class ConDevice
    {
        public string ConnectID { get; set; } = "";
        public string SN { get; set; } = "";
        public int CommDeviceID { get; set; }
        public List<SensorDevice> SensorDevices { get; set; } = new List<SensorDevice>();
        public int State { get; set; }
        public int ProductLineId { get; set; }

    }
    class SensorDevice
    {
        public Device Device { get; set; }
        public AlarmRule AlarmRule { get; set; }
        public SensorValue SensorValue { get; set; }
    }

    class SensorValue
    {
        public decimal Value { get; set; }
        public string Name { get; set; } = "";
        public string State { get; set; } = "正常";
        public int Channel { get; set; }
    }
}
