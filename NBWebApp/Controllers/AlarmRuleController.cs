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
    /// 报警规则接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AlarmRuleController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<AlarmRuleController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AlarmRuleController(NBDataContext context, ILogger<AlarmRuleController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 获取所有规则
        /// </summary>
        /// <returns></returns>
        // GET: api/AlarmRule
        [HttpGet]
        public ActionResult GetAlarmRules()
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.AlarmRules.ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 根据类型获取所有规则
        /// </summary>
        /// <param name="type">类型0：温度  1：压力  2：流量  3：震动</param>
        /// <returns></returns>
        [HttpGet("TypeRules")]
        public ActionResult TypeRules(AlarmType type)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.AlarmRules.Where(t => t.Type == type).ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定ID规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/AlarmRule/5
        [HttpGet("{id}")]
        public ActionResult GetAlarmRule(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.AlarmRules.Find(id);
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 添加/修改规则
        /// </summary>
        /// <param name="alarmRule"></param>
        /// <returns></returns>
        // POST: api/AlarmRule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostGatherRule(AlarmRule alarmRule)
        {
            try
            {
                if (alarmRule.Id > 0)
                {
                    var pl = _context.AlarmRules.Find(alarmRule.Id);
                    if (pl != null)
                    {
                        ObjectCopy.TransReflection(alarmRule, pl);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0, Data = pl });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "规则ID不存在" });
                    }
                }
                else
                {
                    _context.AlarmRules.Add(alarmRule);
                    _context.SaveChanges();
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
        /// 删除指定ID规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/AlarmRule/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGatherRule(int id)
        {
            try
            {
                var pl = _context.AlarmRules.Find(id);
                if (pl != null)
                {
                    _context.AlarmRules.Remove(pl);
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = pl });
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 2000, Message = "规则ID不存在" });
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
