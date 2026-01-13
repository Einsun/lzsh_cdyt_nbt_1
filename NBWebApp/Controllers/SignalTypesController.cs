using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 信号类型
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SignalTypesController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<SignalTypesController> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SignalTypesController(NBDataContext context, ILogger<SignalTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        // GET: api/SignalTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SignalType>>> GetSignalType()
        {
            try
            {
                var _data = await _context.SignalType.ToListAsync();
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _data;

                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 获取指定ID类型详情
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns></returns>
        // GET: api/SignalTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SignalType>> GetSignalType(int id)
        {
            try
            {
                var _data = await _context.SignalType.FindAsync(id);
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _data;

                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        /// <param name="signalType">类型信息</param>
        /// <returns></returns>
        // POST: api/SignalTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SignalType>> PostSignalType(SignalType signalType)
        {
            try
            {
                if (signalType.Id > 0)
                {
                    _context.Entry(signalType).State = EntityState.Modified;
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = signalType.Id });
                }
                else
                {
                    _context.SignalType.Add(signalType);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = signalType.Id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 删除指定ID类型
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <returns></returns>
        // DELETE: api/SignalTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSignalType(int id)
        {
            try
            {
                var signalType = await _context.SignalType.FindAsync(id);
                _context.SignalType.Remove(signalType);
                await _context.SaveChangesAsync();
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = signalType;

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
