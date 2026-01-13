namespace NBWebApp.Models
{
    /// <summary>
    /// 产线
    /// </summary>
    public class ProductLine
    {
        /// <summary>
        /// KEY
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 产线名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 产线图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 检测等级
        /// </summary>
        public string Level { get; set; } = "";
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; } = "";
        /// <summary>
        /// 设备寿命
        /// </summary>
        public string Life { get; set; } = "";
        /// <summary>
        /// 安装时间
        /// </summary>
        public DateTime? InstallTime { get; set; }
    }
    /// <summary>
    /// 产线保存
    /// </summary>
    public class ProductLineSave : ProductLine
    {
        /// <summary>
        /// 图片文件
        /// </summary>
        public IFormFile File { get; set; }
    }
}
