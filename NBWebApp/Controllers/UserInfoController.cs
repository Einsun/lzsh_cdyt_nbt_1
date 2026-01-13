using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBWebApp.Models;
using NBWebApp.Utils;
using System.Security.Cryptography;
using System.Text;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        private readonly NBDataContext _dbContext;
        private readonly ILogger<UserInfoController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtHelper"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UserInfoController(JwtHelper jwtHelper, ILogger<UserInfoController> logger, NBDataContext context)
        {
            _jwtHelper = jwtHelper;
            _logger = logger;
            _dbContext = context;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromForm] string user, [FromForm] string password)
        {
            try
            {
                var pwd = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
                var use = _dbContext.UserInfos.Where(t => t.Name == user && t.Password == pwd).FirstOrDefault();
                if (use!=null)
                {
                    _logger.LogInformation("{}-{}", user, password);
                    var token = _jwtHelper.CreateToken(user, use.Role);
                    return new JsonResult(new ResultInfo() { Code = 0, Data = new { Token = token, role = use.Role } });
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 10, Message = "用户名密码错误" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpassword">原密码</param>
        /// <param name="newpassword">新密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromForm] string oldpassword, [FromForm] string newpassword)
        {
            try
            {
                var user = HttpContext.User.Identity?.Name;
                if (!string.IsNullOrEmpty(user))
                {
                    var pwd = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(oldpassword))).Replace("-", "");
                    var usr = _dbContext.UserInfos.Where(t => t.Name == user && t.Password == pwd).FirstOrDefault();
                    if (usr != null)
                    {
                        _logger.LogInformation("{}-{}-{}", user, oldpassword, newpassword);
                        pwd = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(newpassword))).Replace("-", "");
                        usr.Password = pwd;
                        _dbContext.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0 });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 10, Message = "用户名密码错误" });
                    }
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 10, Message = "用户名密码错误" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddUser")]
        public IActionResult AddUser([FromForm] string user, [FromForm] string password, [FromForm] string role)
        {
            try
            {
                var name = HttpContext.User.Identity?.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    _logger.LogInformation("{}-{}-{}", name, user, password);
                    if (!_dbContext.UserInfos.Any(t => t.Name == user))
                    {
                        var pwd = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
                        var usr = new UserInfo();
                        usr.Name = user;
                        usr.Password = pwd;
                        usr.Role = role;
                        _dbContext.Add(usr);
                        _dbContext.SaveChanges();
                        return new JsonResult(new ResultInfo() { Code = 0 });
                    }
                    else
                    {
                        return new JsonResult(new ResultInfo() { Code = 11, Message = "用户名已存在" });
                    }
                }
                else
                {
                    return new JsonResult(new ResultInfo() { Code = 10, Message = "用户名密码错误" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{}", ex);
                return new JsonResult(new ResultInfo() { Code = 1000, Message = "系统错误" });
            }
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("ListRoles")]
        public IActionResult ListRoles()
        {
            ResultInfo rst = new ResultInfo();
            rst.Code = 0;
            rst.Data = new List<string>() { "Admin","User1", "User2" };

            return new JsonResult(rst);

        }

        /// <summary>
        /// 删除指定ID用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        // DELETE: api/DeleteUserInfo/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            try
            {
                var signalType = await _dbContext.UserInfos.FindAsync(id);
                _dbContext.UserInfos.Remove(signalType);
                await _dbContext.SaveChangesAsync();
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
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        // GET: api/SignalTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GeUserInfo()
        {
            try
            {
                var _data = await _dbContext.UserInfos.ToListAsync();
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
    }
}
