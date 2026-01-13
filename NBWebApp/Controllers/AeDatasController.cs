using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBWebApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 声发射数据接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AeDatasController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<AeDatasController> _logger;
        private readonly IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public AeDatasController(NBDataContext context, ILogger<AeDatasController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }
        /// <summary>
        /// 获取分页声发射数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">个数</param>
        /// <returns></returns>
        [HttpGet("PageData")]
        public ActionResult PageData(int page, int pagecount)
        {
            try
            {

                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Count = _context.AeDatas.Count();
                rst.Data = _context.AeDatas.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 获取分页声发射波形数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">个数</param>
        /// <param name="deviceid">设备id</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">截止时间</param>
        /// <returns></returns>
        [HttpGet("PageWaveData")]
        public ActionResult PageWaveData(int page, int pagecount, int deviceid, string starttime, string endtime)
        {
            try
            {
                var data = from c in _context.AeWaveDatas
                           join d in _context.Devices on c.DeviceId equals d.Id
                           select new
                           {
                               c.Id,
                               c.DeviceId,
                               d.Name,
                               c.SN,
                               c.Points,
                               c.InsertTime,
                           };
                //var data = _context.AeWaveDatas.Where(t=>1==1);
                if (deviceid > 0)
                {
                    data = data.Where(t => t.DeviceId == deviceid);
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    if (DateTime.TryParse(starttime, out var time))
                    {
                        data = data.Where(t => t.InsertTime > time);
                    }
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    if (DateTime.TryParse(endtime, out var time))
                    {
                        data = data.Where(t => t.InsertTime < time);
                    }
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Count = data.Count();
                rst.Data = data.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取实时声发射波形数据
        /// </summary>
        /// <param name="deviceid">设备ID</param>
        /// <returns></returns>
        [HttpGet("LiveWaveData")]
        public ActionResult LiveWaveData(int deviceid)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                var aedata = _context.AeWaveDatas.Where(t => t.DeviceId == deviceid).OrderByDescending(t => t.Id).Take(1).FirstOrDefault();
                if (aedata != null && !string.IsNullOrEmpty(aedata.Data))
                {
                    string savepath = _config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;

                    savepath = Path.Combine(savepath, deviceid.ToString());
                    var filename = Path.Combine(savepath, aedata.Data);
                    if (System.IO.File.Exists(filename))
                    {
                        var data = System.IO.File.ReadAllText(filename);
                        var wave = JsonNode.Parse(data);
                        rst.Code = 0;
                        rst.Data = new
                        {
                            aedata.DeviceId,
                            aedata.Points,
                            aedata.Speed,
                            aedata.Gain,
                            aedata.Enlarge,
                            data = wave
                        };
                        return new JsonResult(rst);
                    }
                    rst.Code = 1;
                    return new JsonResult(rst);
                }
                else
                {
                    rst.Code = 1;
                    return new JsonResult(rst);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定id声发射波形数据
        /// </summary>
        /// <param name="id">声发射波形数据id</param>
        /// <returns></returns>
        [HttpGet("AeWaveData")]
        public ActionResult AeWaveData(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                var aedata = _context.AeWaveDatas.Find(id);
                if (aedata != null && !string.IsNullOrEmpty(aedata.Data))
                {
                   var RootMeanSquare = _context.AeWaveDatas.Where(t=>t.InsertTime>DateTime.Now.Date&t.DeviceId==aedata.DeviceId).Select(t=>t.RootMeanSquare).ToList();
                    string savepath = _config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;

                    savepath = Path.Combine(savepath, aedata.DeviceId.ToString());
                    var filename = Path.Combine(savepath, aedata.Data);
                    if (System.IO.File.Exists(filename))
                    {
                        var data = System.IO.File.ReadAllText(filename);
                        var wave = data.Split(',').Select(t => Convert.ToSingle(t)).ToList();
                        //var wave = JsonSerializer.Deserialize<List<int>>(data);
                        int m = wave.Count() / 4000;
                        var wavenew = new List<float>();
                        for (int i = 0; i < wave.Count(); i = i + m)
                        {
                            wavenew.Add(wave[i]);
                        }
                        
                        //var wave = JsonNode.Parse(data);
                        rst.Code = 0;
                        rst.Data = new
                        {
                            aedata.DeviceId,
                            aedata.Points,
                            aedata.Speed,
                            aedata.Gain,
                            aedata.Enlarge,
                            data = wavenew,
                            aedata.FaultSta,
                            aedata.FaultEnum,
                            WaveImage="wave/"+Path.GetFileName(aedata.MesDataImage1),
                            RootMeanSquare
                        };
                        return new JsonResult(rst);
                    }
                    rst.Code = 1;
                    return new JsonResult(rst);
                }
                else
                {
                    rst.Code = 1;
                    return new JsonResult(rst);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 声发射波形数据下载
        /// </summary>
        /// <param name="id">声发射波形数据ID</param>
        /// <returns></returns>
        [HttpGet("Download")]
        [Authorize(Roles = "Admin,User1")]
        public ActionResult Download(int id)
        {
            var wavedata = _context.AeWaveDatas.Find(id);
            if (wavedata != null)
            {
                var starttime = wavedata.StartTime;
                var endtime = wavedata.EndTime;
                var dev = _context.Devices.Find(wavedata.DeviceId);
                if (dev != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("模拟量采集");
                    dev.ProductLineId = 23;
                    var comms = _context.CommDevices.Where(t => t.ProductLineId == dev.ProductLineId && t.Type == CommType.Gateway).ToList();
                    foreach (var com in comms)
                    {
                        stringBuilder.AppendLine($"传输设备,{com.Name},SN,{com.SN}");
                        stringBuilder.AppendLine("名称,值,采集时间");
                        var sensordata = _context.SensorDatas.Where(t => t.CommDeviceId == com.Id && t.Time >= starttime && t.Time <= endtime);
                        foreach (var sensor in sensordata)
                        {
                            var json = JsonNode.Parse(sensor.Data);
                            var data = json.AsArray();
                            foreach (var d in data)
                            {
                                if (d != null)
                                {
                                    stringBuilder.AppendLine($"{d["name"]},{d["value"]},{sensor.Time}");
                                }
                            }
                        }
                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("声发射采集");
                    var rule = _context.AeRules.Find(dev.AeRuleId);
                    if (rule != null)
                    {
                        var json = JsonNode.Parse(rule.TimingConfig);
                        if (json != null)
                        {
                            stringBuilder.AppendLine($"采集时间,{wavedata.InsertTime},采集时长,{json["ae_timing_length"]}");
                            stringBuilder.AppendLine("采集参数");
                            json = JsonNode.Parse(rule.MeasureConfig);
                            if (json != null)
                            {
                                stringBuilder.AppendLine("采集门限,采集速率");
                                stringBuilder.AppendLine($"{json["ae_threshold"]},{json["ae_measure_speed"]}");
                                stringBuilder.AppendLine("采集数据");
                                string savepath = _config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;

                                savepath = Path.Combine(savepath, wavedata.DeviceId.ToString());
                                var filename = Path.Combine(savepath, wavedata.Data);
                                if (System.IO.File.Exists(filename))
                                {
                                    var data = System.IO.File.ReadAllText(filename);
                                    stringBuilder.AppendLine(data.Trim('[').Trim(']').Replace(",", Environment.NewLine));
                                }

                                MemoryStream ms = new MemoryStream();
                                ms.Write(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
                                ms.Flush();
                                ms.Position = 0;
                                stringBuilder.Clear();
                                stringBuilder = null;
                                return new FileStreamResult(ms, "text/csv") { FileDownloadName = $"{dev.Name}-{DateTime.Now:yyyy-MM-dd HH:mm:ss}.csv" };
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// 下载多个id声发射数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("DownloadMuti")]
        [Authorize(Roles = "Admin,User1")]
        public ActionResult DownloadMuti(int[] ids)
        {
            try
            {
                var zipMs = new MemoryStream();

                
                ZipOutputStream zipStream = new ZipOutputStream(zipMs);
                {
                    zipStream.SetLevel(9);
                    foreach (var id in ids)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        var wavedata = _context.AeWaveDatas.Find(id);
                        if (wavedata != null)
                        {
                            var starttime = wavedata.StartTime;
                            var endtime = wavedata.EndTime;
                            var dev = _context.Devices.Find(wavedata.DeviceId);
                            if (dev != null)
                            {
                                stringBuilder.AppendLine("模拟量采集");
                                dev.ProductLineId = 23;
                                var comms = _context.CommDevices.Where(t => t.ProductLineId == dev.ProductLineId && t.Type == CommType.Gateway).ToList();
                                foreach (var com in comms)
                                {
                                    stringBuilder.AppendLine($"传输设备,{com.Name},SN,{com.SN}");
                                    stringBuilder.AppendLine("名称,值,采集时间");
                                    var sensordata = _context.SensorDatas.Where(t => t.CommDeviceId == com.Id && t.Time >= starttime && t.Time <= endtime);
                                    foreach (var sensor in sensordata)
                                    {
                                        var json = JsonNode.Parse(sensor.Data);
                                        var data = json.AsArray();
                                        foreach (var d in data)
                                        {
                                            if (d != null)
                                            {
                                                stringBuilder.AppendLine($"{d["name"]},{d["value"]},{sensor.Time}");
                                            }
                                        }
                                    }
                                    stringBuilder.AppendLine();
                                }
                                stringBuilder.AppendLine();
                                stringBuilder.AppendLine("声发射采集");
                                var rule = _context.AeRules.Find(dev.AeRuleId);
                                if (rule != null)
                                {
                                    var json = JsonNode.Parse(rule.TimingConfig);
                                    if (json != null)
                                    {
                                        stringBuilder.AppendLine($"采集时间,{wavedata.InsertTime},采集时长,{json["ae_timing_length"]}");
                                        stringBuilder.AppendLine("采集参数");
                                        json = JsonNode.Parse(rule.MeasureConfig);
                                        if (json != null)
                                        {
                                            stringBuilder.AppendLine("采集门限,采集速率");
                                            stringBuilder.AppendLine($"{json["ae_threshold"]},{json["ae_measure_speed"]}");
                                            stringBuilder.AppendLine("采集数据");
                                            string savepath = _config.GetSection("AEData").Value ?? AppDomain.CurrentDomain.BaseDirectory;

                                            savepath = Path.Combine(savepath, wavedata.DeviceId.ToString());
                                            var filename = Path.Combine(savepath, wavedata.Data);
                                            if (System.IO.File.Exists(filename))
                                            {
                                                var data = System.IO.File.ReadAllText(filename);
                                                stringBuilder.AppendLine(data.Trim('[').Trim(']').Replace(",", Environment.NewLine));
                                            }
                                        }
                                    }
                                }
                            }
                            zipStream.PutNextEntry(new ZipEntry($"{id}-{DateTime.Now:yyyy-MM-dd HH:mm:ss}.csv"));
                            var _data = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                            zipStream.Write(_data, 0, _data.Length);
                            zipStream.Flush();

                        }
                    }
                    zipStream.Flush();
                    zipStream.Finish();
                    zipMs.Position = 0;
                    //zipMs.CopyTo(returnStream, 5600);
                    return new FileStreamResult(zipMs, "application/zip") { FileDownloadName = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}.zip" };

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return NotFound("系统错误");
            }
        }
    }
}
