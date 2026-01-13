using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;
using NBWebApp.Utils;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 设备接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<DevicesController> _logger;
        private readonly RedisHelper _redis;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="redis"></param>
        public DevicesController(NBDataContext context, ILogger<DevicesController> logger, RedisHelper redis)
        {
            _context = context;
            _logger = logger;
            _redis = redis;
        }
        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        /// <returns></returns>
        // GET: api/Devices
        [HttpGet]
        public ActionResult GetDevices()
        {
            try
            {
                var queru = from u in _context.Devices
                            join p in _context.ProductLines on u.ProductLineId equals p.Id
                            join s in _context.CommDevices on u.CommDeviceId equals s.Id
                            select new
                            {
                                u.ProductLineId,
                                ProductLineName = p.Name,
                                u.CommDeviceId,
                                CommDeviceName = s.Name,
                                DeviceId = u.Id,
                                DeviceName = u.Name
                            };
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = queru.ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取产线相应ID下type类型的所有设备
        /// </summary>
        /// <param name="id">产线ID</param>
        /// <param name="type">设备类型0:声发射 ; 1:温度传感器 ;2 压力传感器;3 流量传感器;4 振动设备</param>
        /// <returns></returns>
        [HttpGet("ProductLine")]
        public ActionResult ProductLine(int id, GatherType type)
        {
            try
            {
                var data = _context.Devices.Where(t => t.ProductLineId == id && t.Type == type).ToList();

                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data;
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定传输设备ID下所有设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns></returns>
        // GET: api/Devices/5
        [HttpGet("CommDevice/{id}")]
        public ActionResult CommDevice(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.Devices.Where(t => t.CommDeviceId == id).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定ID设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns></returns>
        // GET: api/Devices/5
        [HttpGet("{id}")]
        public ActionResult GetDevice(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.Devices.Find(id);
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 添加/修改传感器设备
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        // POST: api/Devices/AEDevice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostDevice(Device device)
        {
            try
            {
                JsonSerializerOptions jsonoption = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
                _logger.LogInformation("{}", JsonSerializer.Serialize(device));
                if (!_context.Devices.Any(t => t.CommDeviceId == device.CommDeviceId
                    && t.Pos == device.Pos && t.Address == device.Address && t.Id != device.Id))
                {
                    if (device.ProductLineId == 0 || device.CommDeviceId == 0)
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "产线/传输设备无效" });
                    }
                    else if (device.Id > 0)
                    {
                        var pl = _context.Devices.Find(device.Id);
                        if (pl != null)
                        {
                            ObjectCopy.TransReflection(device, pl);
                            _context.SaveChanges();
                            return new JsonResult(new ResultInfo() { Code = 0, Data = pl });
                        }
                        else
                        {
                            return new JsonResult(new ResultInfo() { Code = 2000, Message = "设备ID不存在" });
                        }
                    }
                    else
                    {
                        _context.Devices.Add(device);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0 });
                    }
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 2000, Message = "已添加相同通道地址的设备" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 添加/修改声发射设备
        /// </summary>
        /// <param name="device">设备信息</param>
        /// <returns></returns>
        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AEDevice")]
        [Authorize(Roles = "Admin")]
        public ActionResult AEDevice(AEDevice device)
        {
            try
            {
                JsonSerializerOptions jsonoption = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
                _logger.LogInformation("{}", JsonSerializer.Serialize(device));
                if (device.ProductLineId == 0 || device.CommDeviceId == 0)
                {
                    return new JsonResult(new ResultInfo() { Code = 2000, Message = "产线/传输设备无效" });
                }
                else if (device.Id > 0)
                {
                    var pl = _context.Devices.Find(device.Id);
                    if (pl != null)
                    {
                        pl.Name = device.Name;
                        pl.Type = GatherType.AE;
                        pl.ProductLineId = device.ProductLineId;
                        pl.CommDeviceId = device.CommDeviceId;
                        pl.SN = device.SN;
                        _redis.GetDatabase().ListLeftPush("device", device.Id);
                        pl.AeRuleId = device.AeRuleId;
                        // pl.AEInfo=System.Text.Json.JsonSerializer.Serialize(device.AeInfo);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0, Data = pl });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "设备ID不存在" });
                    }
                }
                else
                {
                    var dv = new Device();
                    dv.Name = device.Name;
                    dv.Type = GatherType.AE;
                    dv.SN = device.SN;
                    dv.ProductLineId = device.ProductLineId;
                    dv.CommDeviceId = device.CommDeviceId;
                    dv.AeRuleId = device.AeRuleId;
                    //dv.AEInfo= System.Text.Json.JsonSerializer.Serialize(device.AeInfo);
                    _context.Devices.Add(dv);
                    _context.SaveChanges();
                    Task.Factory.StartNew(() =>
                    {
                        _redis.GetDatabase().ListLeftPush("device", device.Id);
                    });
                    return new JsonResult(new ResultInfo() { Code = 0 });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 删除指定ID设备
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns></returns>
        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDevice(int id)
        {
            try
            {
                var pl = _context.Devices.Find(id);
                if (pl != null)
                {
                    _context.Devices.Remove(pl);
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = pl });
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 2000, Message = "产线ID不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 产线设备个数统计
        /// </summary>
        /// <param name="prodecelineid"></param>
        /// <returns></returns>
        [HttpGet("DeviceSummary")]
        public ActionResult DeviceSummary(int prodecelineid)
        {
            try
            {
                var _data = _context.Devices.Where(t => t.ProductLineId == prodecelineid);
                int total = _data.Count();
                var data = _data.GroupBy(t => t.Type)
                 .Select(d => new
                 {
                     percent = Math.Round(d.Count() * 1.0 / total, 4),
                     type = (int)d.Key
                 });
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = data.ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 采集一次
        /// </summary>
        /// <param name="sn">传输设备SN</param>
        /// <returns></returns>
        [HttpGet("CaptureNow")]
        public ActionResult CaptureNow(string sn)
        {
            var se = ServiceLocator.Instance.GetServices<IHostedService>();
            foreach (var s in se)
            {
                var t = s as ModBusService;
                if (t != null)
                {
                    t.SendOne(sn);
                }
            }
            return Ok();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// 
        /// </summary>
        public static IServiceProvider Instance { get; set; }
    }
}
