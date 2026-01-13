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
    /// 采集规则接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GatherRuleController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<GatherRuleController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public GatherRuleController(NBDataContext context, ILogger<GatherRuleController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 获取所有传感器采集规则
        /// </summary>
        /// <returns></returns>
        // GET: api/GatherRule
        [HttpGet]
        public ActionResult GetGatherRules()
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.GatherRules.ToList();
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
        // GET: api/GatherRule/5
        [HttpGet("{id}")]
        public ActionResult GetGatherRule(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.GatherRules.Find(id);
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
        /// <param name="gatherRule"></param>
        /// <returns></returns>
        // POST: api/GatherRule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostGatherRule(GatherRule gatherRule)
        {
            try
            {
                if (gatherRule.Id > 0)
                {
                    var pl = _context.GatherRules.Find(gatherRule.Id);
                    if (pl != null)
                    {
                        ObjectCopy.TransReflection(gatherRule, pl);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0, Data = pl.Id });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "规则ID不存在" });
                    }
                }
                else
                {
                    _context.GatherRules.Add(gatherRule);
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = gatherRule.Id });
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
        // DELETE: api/GatherRule/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGatherRule(int id)
        {
            try
            {
                var pl = _context.GatherRules.Find(id);
                if (pl != null)
                {
                    _context.GatherRules.Remove(pl);
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
