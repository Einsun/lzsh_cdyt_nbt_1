namespace NBWebApp.Models
{
    /// <summary>
    /// 报警规则
    /// </summary>
    public class AeData
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备SN
        /// </summary>
        public string SN { get; set; } = "";
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 幅度
        /// </summary>
        public double AMP { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public double ASL { get; set; }
        /// <summary>
        /// 能量
        /// </summary>
        public double Power { get; set; }
        /// <summary>
        /// 有效值
        /// </summary>
        public double RMS { get; set; }
        /// <summary>
        /// 上升时间
        /// </summary>
        public int RisingTime { get; set; }
        /// <summary>
        /// 上升计数
        /// </summary>
        public int RisingNum { get; set; }
        /// <summary>
        /// 振铃计数
        /// </summary>
        public int RingingNum { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public int DuringTime { get; set; }
        /// <summary>
        /// unix时间戳
        /// </summary>
        public double UnixTime { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }=DateTime.Now;
    }

}
