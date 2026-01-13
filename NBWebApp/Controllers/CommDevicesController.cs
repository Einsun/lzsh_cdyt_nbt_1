using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 传输设备接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CommDevicesController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<CommDevicesController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public CommDevicesController(NBDataContext context, ILogger<CommDevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 获取所有传输设备
        /// </summary>
        /// <returns></returns>
        // GET: api/CommDevices
        [HttpGet]
        public ActionResult GetCommDevice()
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.CommDevices.ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定产线ID下所有传输设备
        /// </summary>
        /// <returns></returns>
        // GET: api/CommDevices
        [HttpGet("ProductLine/{id}")]
        public ActionResult ProductLine(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.CommDevices.Where(t => t.ProductLineId == id).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取产线指定类型传输设备
        /// </summary>
        /// <param name="id">产线id</param>
        /// <param name="type">传输设备类型</param>
        /// <returns></returns>
        [HttpGet("TypeCommDevice")]
        public ActionResult TypeCommDevice(int id,CommType type)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.CommDevices.Where(t => t.ProductLineId == id&&t.Type==type).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定ID传输设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/CommDevices/5
        [HttpGet("{id}")]
        public ActionResult GetCommDevice(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.CommDevices.Find(id);
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 添加/修改传输设备
        /// </summary>
        /// <param name="commDevice"></param>
        /// <returns></returns>
        // POST: api/CommDevices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostCommDevice(CommDevice commDevice)
        {
            try
            {
                if (commDevice.Id > 0)
                {
                    var pl = _context.CommDevices.Find(commDevice.Id);
                    if (pl != null)
                    {
                        if(_context.CommDevices.Any(t=>t.SN == commDevice.SN && t.Id != commDevice.Id))
                        {
                            return new JsonResult(new ResultInfo() { Code = 2000, Message = "SN已存在" });
                        }
                        else
                        {
                            ObjectCopy.TransReflection(commDevice, pl);
                            _context.SaveChanges();
                            return new JsonResult(new ResultInfo() { Code = 0, Data = pl.Id });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "产线ID不存在" });
                    }
                }
                else
                {
                    if (_context.CommDevices.Any(t => t.SN == commDevice.SN))
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "SN已存在" });
                    }
                    else
                    {
                        _context.CommDevices.Add(commDevice);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0, Data = commDevice.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 删除指定ID传输设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/CommDevices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCommDevice(int id)
        {
            try
            {
                var pl = _context.CommDevices.Find(id);
                if (pl != null)
                {
                    _context.CommDevices.Remove(pl);
                    var devs= _context.Devices.Where(t => t.CommDeviceId == id);
                    _context.Devices.RemoveRange(devs);
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

    }
}
