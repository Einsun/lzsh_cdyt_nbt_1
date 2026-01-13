using System.Security.Cryptography;
using System.Text;

namespace NBWebApp.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
        /// <summary>
        /// 角色 Admin  User1  User2
        /// </summary>
        public string Role { get; set; } = "Admin";
    }
}
