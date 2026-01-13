using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBWebApp.Models
{
    /// <summary>
    /// 信号类型
    /// </summary>
    public class SignalType
    {
        /// <summary>
        /// KEY
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = "";
        /// <summary>
        /// 最大值
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        [Column(TypeName = "decimal(5, 2)")]
        public decimal MinValue { get; set; }
    }
}
