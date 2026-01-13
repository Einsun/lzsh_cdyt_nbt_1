namespace NBWebApp.Models
{
    /// <summary>
    /// 报警数据
    /// </summary>
    public class AlarmData
    {
        /// <summary>
        /// key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 报警原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Settle { get; set; }
        /// <summary>
        /// 类型 0 正常;1 报警;2 故障
        /// </summary>
        public int AlarmType {  get; set; }
    }

}
