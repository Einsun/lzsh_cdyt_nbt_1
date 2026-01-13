using System;
using System.Collections.Generic;
using System.IO;
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
    /// 产线接口
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductLineController : ControllerBase
    {
        private readonly NBDataContext _context;
        private readonly ILogger<ProductLineController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public ProductLineController(ILogger<ProductLineController> logger, NBDataContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductLine
        /// <summary>
        /// 获取所有产线信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductLines()
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.ProductLines.ToList();
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        // GET: api/ProductLine/5
        /// <summary>
        /// 获取指定ID产线信息
        /// </summary>
        /// <param name="id">产线ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetProductLine(int id)
        {
            try
            {
                ResultInfo rst = new ResultInfo();
                rst.Code = 0;
                rst.Data = _context.ProductLines.Find(id);
                return new JsonResult(rst);
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        /// <summary>
        /// 添加/修改产线
        /// </summary>
        /// <param name="productLine">产线存储对象</param>
        /// <returns></returns>
        // POST: api/ProductLine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PostProductLine([FromForm] ProductLineSave productLine)
        {
            try
            {
                if (productLine.File != null)
                {
                    string savepath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Image");
                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }
                    var filename = Guid.NewGuid().ToString("N") + Path.GetExtension(productLine.File.FileName);
                    Console.WriteLine(savepath);

                    savepath = Path.Combine(savepath, filename);
                    using (var stream = new FileStream(savepath, FileMode.Create))
                    {
                        productLine.File.CopyTo(stream);
                    }
                    productLine.Image = $"/Image/{filename}";
                }
                if (productLine.Id > 0)
                {
                    var pl = _context.ProductLines.Find(productLine.Id);
                    if (pl != null)
                    {
                        ObjectCopy.TransReflection(productLine, pl);
                        _context.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0, Data = pl.Id });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 2000, Message = "产线ID不存在" });
                    }
                }
                else
                {
                    var pl = (ProductLine)productLine;
                    _context.ProductLines.Add(pl);
                    _context.SaveChanges();
                    return new JsonResult(new ResultInfo() { Code = 0, Data = pl.Id });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }

        // DELETE: api/ProductLine/5
        /// <summary>
        /// 删除指定ID产线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProductLine(int id)
        {
            try
            {
                var pl = _context.ProductLines.Find(id);
                if (pl != null)
                {
                    _context.ProductLines.Remove(pl);
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
