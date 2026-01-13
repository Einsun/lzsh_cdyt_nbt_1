using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBWebApp.Models;
using NBWebApp.Utils;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 声发射规则
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AeRulesController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<AeRulesController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AeRulesController(NBDataContext context, ILogger<AeRulesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有规则
        /// </summary>
        /// <returns></returns>
        // GET: api/AeRules
        [HttpGet]
        public ActionResult GetAeRules()
        {
            try
            {
                var _data=_context.AeRules.ToList();
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _data.Select(t =>
                {
                    return new
                    {
                        t.Id,
                        t.Name,
                        MeasureConfig = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(t.MeasureConfig),
                        LevelConfig = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(t.LevelConfig),
                        TimingConfig = System.Text.Json.JsonSerializer.Deserialize<JsonObject>(t.TimingConfig),
                    };
                });
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
        // GET: api/AeRules/5
        [HttpGet("{id}")]
        public ActionResult GetAeRule(int id)
        {
            try
            {
                var _data=_context.AeRules.Find(id);
                if (_data != null)
                {
                    ResultInfo rst = new ResultInfo();
                    rst.Code = 0;
                    rst.Data = new
                    {
                        _data.Id,
                        _data.Name,
                        MeasureConfig = JsonSerializer.Deserialize<JsonObject>(_data.MeasureConfig),
                        LevelConfig = JsonSerializer.Deserialize<JsonObject>(_data.LevelConfig),
                        TimingConfig = JsonSerializer.Deserialize<JsonObject>(_data.TimingConfig),
                    };
                    return new JsonResult(rst);
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 2000, Message = "无效ID" });
                }
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
        /// <param name="aeRule">采集规则</param>
        /// <returns></returns>
        // POST: api/AeRules
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostAeRule(AeDataRule aeRule)
        {
            try
            {
                if (aeRule.Id > 0)
                {
                    var pl = _context.AeRules.Find(aeRule.Id);
                    if (pl != null)
                    {
                        pl.Name = aeRule.Name;
                        pl.MeasureConfig = JsonSerializer.Serialize(aeRule.measureConfig);
                        pl.TimingConfig = JsonSerializer.Serialize(aeRule.timingConfig);
                        pl.LevelConfig = JsonSerializer.Serialize(aeRule.levelConfig);
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
                    var _aeRule=new AeRule();
                    _aeRule.Name = aeRule.Name;
                    _aeRule.MeasureConfig = JsonSerializer.Serialize(aeRule.measureConfig);
                    _aeRule.TimingConfig = JsonSerializer.Serialize(aeRule.timingConfig);
                    _aeRule.LevelConfig = JsonSerializer.Serialize(aeRule.levelConfig);
                    _context.AeRules.Add(_aeRule);
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = _aeRule.Id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 删除指定ID采集规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/AeRules/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAeRule(int id)
        {
            try
            {
                var pl = _context.AeRules.Find(id);
                if (pl != null)
                {
                    _context.AeRules.Remove(pl);
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
