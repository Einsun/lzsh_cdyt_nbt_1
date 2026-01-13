namespace NBWebApp.Models
{
    /// <summary>
    /// 传输设备
    /// </summary>
    public class CommDevice
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 产线ID
        /// </summary>
        public int ProductLineId { get; set; }
        /// <summary>
        /// 传输设备类型
        /// </summary>
        public CommType Type { get; set; }
        /// <summary>
        /// 设备SN
        /// </summary>
        public string SN { get; set; } = "";
        /// <summary>
        /// 在线状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 传感器采集规则
        /// </summary>
        public int? GatherRuleId { get; set; }
    }

    /// <summary>
    /// 传输设备类型 0:采集网关; 1:声发射
    /// </summary>
    public enum CommType
    {
        /// <summary>
        /// 采集网关
        /// </summary>
        Gateway,
        /// <summary>
        /// 声发射
        /// </summary>
        AE
    }
}
