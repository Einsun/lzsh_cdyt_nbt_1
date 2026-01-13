using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NBWebApp.Utils
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public string CreateToken(string user,string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user), //HttpContext.User.Identity.Name
                new Claim(ClaimTypes.Role, role), //HttpContext.User.IsInRole("r_admin")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "lanyexingkong@ilsky.com"));
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],     //Issuer
                _configuration["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddSeconds(86400),    //expires
                signingCredentials               //Credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
