namespace NBWebApp.Models
{
    /// <summary>
    /// 报警规则
    /// </summary>
    public class AlarmRule
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 设备类型
        /// </summary>
        public AlarmType Type {  get; set; }
        /// <summary>
        /// 上限值
        /// </summary>
        public decimal Max {  get; set; }
        /// <summary>
        /// 下限值
        /// </summary>
        public decimal Min { get; set; }
        /// <summary>
        /// 告警等级
        /// </summary>
        public int Level { get; set; }
    }
    /// <summary>
    /// 设备类型 0：温度  1：压力  2：流量  3：震动 
    /// </summary>
    public enum AlarmType
    {
        /// <summary>
        /// 温度
        /// </summary>
        TEMP,
        /// <summary>
        /// 压力
        /// </summary>
        PRESS,
        /// <summary>
        /// 流量
        /// </summary>
        FLOW,
       /// <summary>
       /// 振动
       /// </summary>
        VIBRATING
    }
}
