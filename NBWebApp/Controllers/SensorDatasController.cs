using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using log4net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 传感器数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDatasController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<AlarmDatasController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SensorDatasController(NBDataContext context, ILogger<AlarmDatasController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 获取分页传感器数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">个数</param>
        /// <param name="commdeviceid">传输设备id</param>
        /// <returns></returns>
        [HttpGet("PageSensorData")]
        public ActionResult PageSensorData(int page, int pagecount, int commdeviceid)
        {
            try
            {
                var data = _context.SensorDatas.Where(t => 1 == 1);
                if (commdeviceid > 0)
                {
                    data = data.Where(t => t.CommDeviceId == commdeviceid);
                }
                var _data = data.OrderByDescending(t => t.Id).Skip((page - 1) * pagecount).Take(pagecount).ToList();
                List<object> list = new List<object>();
                foreach (var item in _data)
                {
                    list.Add(new { data = JsonNode.Parse(item.Data), time = item.Time });
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Count = data.Count();
                rst.Data = list;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 获取传感器最新数据
        /// </summary>
        /// <param name="commdeviceid">传输设备id</param>
        /// <returns></returns>
        [HttpGet("New")]
        public ActionResult New(int commdeviceid)
        {
            try
            {
                var data = _context.SensorDatas.Where(t => 1 == 1);
                if (commdeviceid > 0)
                {
                    data = data.Where(t => t.CommDeviceId == commdeviceid);
                }
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                var _data = data.OrderByDescending(t => t.Id).FirstOrDefault()?.Data;
                if (!string.IsNullOrEmpty(_data))
                {
                    rst.Data = JsonNode.Parse(_data);

                }
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线下设备信息
        /// </summary>
        /// <param name="productlineid">产线id</param>
        /// <returns></returns>
        [HttpGet("ProdectLine")]
        public ActionResult ProdectLine(int productlineid)
        {
            try
            {
                var pl = _context.ProductLines.Find(productlineid);
                dynamic obj = new ExpandoObject();
                obj.level = pl.Level;
                List<object> list = new List<object>();
                var time = DateTime.Now.AddHours(-1);
                var set = _context.AlarmDatas.AsNoTracking().Where(t=>t.Time> time);
                var sorted = set.OrderByDescending(x => x.Id);
                var latestLogs = set.Select(x => x.DeviceId)
                    .Distinct()
                    .SelectMany(x => sorted.Where(y => y.DeviceId == x).Take(1)).ToList();

                var comms = _context.CommDevices.Where(t => t.ProductLineId == productlineid).ToList();
                foreach (var com in comms)
                {
                    var devs = _context.Devices.Where(t => t.CommDeviceId == com.Id).ToList();
                    dynamic comdata = new ExpandoObject();
                    comdata.name = com.Name;
                    comdata.sn = com.SN;
                    comdata.id = com.Id;
                    comdata.state = com.State;
                    List<object> devvalues = new List<object>();
                    if (com.Type == CommType.Gateway)
                    {
                        foreach (var dev in devs)
                        {
                            var alarmrule = _context.AlarmRules.Find(dev.AlarmRuleId);
                            {
                                dynamic devdata = new ExpandoObject();
                                devdata.name = dev.Name;
                                devdata.channel = dev.Pos;
                                devdata.unit = dev.Unit;
                                devdata.level = alarmrule?.Level;
                                var alarmdata=latestLogs.Find(t=>t.DeviceId == dev.Id);
                                //var alarmdata = _context.AlarmDatas.Where(t => t.DeviceId == dev.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                                if (alarmdata == null)
                                {
                                    devdata.value = 0;
                                    devdata.state = "离线";
                                }
                                else
                                {
                                    devdata.value = alarmdata.Value;
                                    switch (alarmdata.AlarmType)
                                    {
                                        case 0:
                                            devdata.state = "正常";
                                            break;
                                        case 1:
                                            devdata.state = "告警";
                                            break;
                                        case 2:
                                            devdata.state = "离线";
                                            break;
                                    }
                                }
                                devvalues.Add(devdata);
                            }
                        }
                    }
                    else
                    {
                        foreach (var dev in devs)
                        {
                            dynamic devdata = new ExpandoObject();
                            devdata.name = dev.Name;
                            devdata.state = dev.State == 0 ? "离线" : "在线";
                            var alarmdata = _context.AeDatas.Where(t => t.DeviceId == dev.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                            if (alarmdata == null)
                            {
                                devdata.value = 0;
                            }
                            else
                            {
                                devdata.value = alarmdata.RMS;
                            }
                            devvalues.Add(devdata);
                        }
                    }
                    comdata.sensordata = devvalues;
                    list.Add(comdata);
                }
                obj.comms = list;
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = obj;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
    }
}
