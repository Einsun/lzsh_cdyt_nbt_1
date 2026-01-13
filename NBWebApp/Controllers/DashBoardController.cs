using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NBWebApp.Models;
using NBWebApp.Utils;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 面板接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<DashBoardController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public DashBoardController(NBDataContext context, ILogger<DashBoardController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 设备汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("DeviceSummary")]
        public IActionResult DeviceSummary()
        {
            try
            {
                int productLine = _context.ProductLines.Count();
                int device = _context.Devices.Count();
                int online = _context.Devices.Where(t => t.State == 1).Count();

                List<DevInfo> devlist = new List<DevInfo>();
                var pls = _context.ProductLines.ToList();
                foreach (var p in pls)
                {
                    var pldev = new DevInfo();
                    pldev.ID = p.Id;
                    pldev.Name = p.Name;
                    pldev.Datas = new List<DevInfo>();

                    var comms = _context.CommDevices.Where(t => t.ProductLineId == p.Id).ToList();
                    foreach (var c in comms)
                    {
                        var cmdev = new DevInfo();
                        cmdev.ID = c.Id;
                        cmdev.Name = c.Name;
                        cmdev.Datas = new List<DevInfo>();

                        var devs = _context.Devices.Where(t => t.CommDeviceId == c.Id).ToList();
                        foreach (var d in devs)
                        {
                            var dev = new DevInfo();
                            dev.ID = d.Id;
                            dev.Name = d.Name;
                            cmdev.Datas.Add(dev);
                        }
                        pldev.Datas.Add(cmdev);
                    }
                    devlist.Add(pldev);
                }

                // var queru = from u in _context.Devices
                //             join p in _context.ProductLines on u.ProductLineId equals p.Id
                //             join s in _context.CommDevices on u.CommDeviceId equals s.Id
                //             select new
                //             {
                //                 u.ProductLineId,
                //                 ProductLineName = p.Name,
                //                 u.CommDeviceId,
                //                 CommDeviceName = s.Name,
                //                 DeviceId = u.Id,
                //                 DeviceName = u.Name
                //             };
                //var devices= queru.OrderBy(t => t.ProductLineId)
                //     .ThenBy(t => t.CommDeviceId)
                //     .ThenBy(t => t.DeviceId).ToList();


                // foreach (var d in devices)
                // {
                //     var pl = devlist.Find(t => t.ID == d.ProductLineId);
                //     if (pl==null)
                //     {
                //         var pldev = new DevInfo();
                //         pldev.ID = d.ProductLineId;
                //         pldev.Name = d.ProductLineName;
                //         pldev.Datas = new List<DevInfo>();

                //         var cmdev = new DevInfo();
                //         cmdev.ID = d.CommDeviceId;
                //         cmdev.Name = d.CommDeviceName;
                //         cmdev.Datas = new List<DevInfo>();

                //         var dev = new DevInfo();
                //         dev.ID = d.DeviceId;
                //         dev.Name = d.DeviceName;
                //         cmdev.Datas.Add(dev);
                //         pldev.Datas.Add(cmdev);
                //         devlist.Add(pldev);
                //     }
                //     else
                //     {
                //         var cm = pl.Datas?.Find(t => t.ID == d.CommDeviceId);
                //         if (cm == null)
                //         {
                //             var cmdev = new DevInfo();
                //             cmdev.ID = d.CommDeviceId;
                //             cmdev.Name = d.CommDeviceName;
                //             cmdev.Datas = new List<DevInfo>();

                //             var dev = new DevInfo();
                //             dev.ID = d.DeviceId;
                //             dev.Name = d.DeviceName;
                //             cmdev.Datas.Add(dev);
                //             pl.Datas?.Add(cmdev);
                //         }
                //         else
                //         {
                //             var dev = new DevInfo();
                //             dev.ID = d.DeviceId;
                //             dev.Name = d.DeviceName;
                //             cm.Datas?.Add(dev);
                //         }
                //     }
                // }

                ResultInfo rst = new ResultInfo();
                rst.Code = 0;

                rst.Data = new { productLine, device, online, devlist };
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                return new JsonResult(rst, options);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 报警等级汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProductLineSummary")]
        public IActionResult ProductLineSummary()
        {
            try
            {
                var data = _context.ProductLines.
                    GroupBy(t => t.Level, (k, g) => new
                    {
                        level = k,
                        total = g.Count()
                    });

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
        /// 报警汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("AlarmSummary")]
        public IActionResult AlarmSummary()
        {
            try
            {
                var data = _context.AlarmDatas.Join(_context.Devices, a => a.DeviceId, b => b.Id, (a, b) => new { a.Id, b.Type })
                    .GroupBy(t => t.Type, (k, g) => new
                    {
                        type = k,
                        total = g.Count()
                    });

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
        /// 设备报警记录
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagecount">每页个数</param>
        /// <returns></returns>
        [HttpGet("AlarmHistory")]
        public IActionResult AlarmHistory(int page, int pagecount)
        {
            try
            {
                var queru =
                    from u in _context.AlarmDatas
                    join p in _context.Devices on u.DeviceId equals p.Id
                    join s in _context.ProductLines on p.ProductLineId equals s.Id
                    select new
                    {
                        ProductLineName = s.Name,
                        DeviceName = p.Name,
                        u.Reason,
                        u.Time,
                        u.Settle
                    };
                var data = queru.OrderByDescending(t => t.Time).Skip((page - 1) * pagecount).Take(pagecount).ToList();

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

    }

}
