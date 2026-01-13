namespace NBWebApp.Models
{
    /// <summary>
    /// 声发射采集规则
    /// </summary>
    public class AeRule
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "{}";
        /// <summary>
        /// 采集设置
        /// </summary>
        public string MeasureConfig { get; set; } = "{}";
        /// <summary>
        /// 评级设置
        /// </summary>
        public string LevelConfig { get; set; } = "{}";
        /// <summary>
        /// 定时设置
        /// </summary>
        public string TimingConfig { get; set; } = "{}";
    }
    /// <summary>
    /// 声发射采集规则
    /// </summary>
    public class AeDataRule
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 声发射采集设置
        /// </summary>
        public AeMeasureConfig measureConfig { get; set; }
        /// <summary>
        /// 声发射评级设置
        /// </summary>
        public AeLevelConfig levelConfig { get; set; }
        /// <summary>
        /// 声发射定时设置
        /// </summary>
        public AeTimingConfig timingConfig { get; set; }

    }
}
