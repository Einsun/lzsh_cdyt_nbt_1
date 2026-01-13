namespace NBWebApp.Models
{
    /// <summary>
    /// 采集规则
    /// </summary>
    public class GatherRule
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; } = "";
        ///// <summary>
        ///// 采集类型
        ///// </summary>
        //public GatherType Type { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 采集间隔，单位秒
        /// </summary>
        public int Interval { get; set; }

    }
    /// <summary>
    /// 采集类型 0:声发射 ; 1:温度传感器 ;2 压力传感器;3 流量传感器;4 振动设备
    /// </summary>
    public enum GatherType
    {
        /// <summary>
        /// 声发射
        /// </summary>
        AE,
        /// <summary>
        /// 温度传感器
        /// </summary>
        TEMPERATURE,
        /// <summary>
        /// 压力传感器
        /// </summary>
        PRESSURE,
        /// <summary>
        /// 流量传感器
        /// </summary>
        FLOWRATE,
        /// <summary>
        /// 振动设备
        /// </summary>
        Vibrating
    }
}
