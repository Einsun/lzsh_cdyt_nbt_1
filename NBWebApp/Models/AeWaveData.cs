namespace NBWebApp.Models
{
    /// <summary>
    /// 声发射波形数据
    /// </summary>
    public class AeWaveData
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
        /// 波形点数
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 采样速率
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// 前放增益
        /// </summary>
        public int Gain { get; set; }
        /// <summary>
        /// 电路放大倍数
        /// </summary>
        public double Enlarge { get; set; }
        /// <summary>
        /// 波形数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// unix时间戳
        /// </summary>
        public double UnixTime { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }= DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 有无故障
        /// </summary>
        public int  FaultSta { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        public string FaultEnum { get; set; }
        /// <summary>
        /// 均方根
        /// </summary>
        public decimal RootMeanSquare { get; set; }
        /// <summary>
        /// 计算后的数据波形1
        /// </summary>
        public string MesDataAddr1 { get; set; }
        /// <summary>
        /// 计算后的数据波形1
        /// </summary>
        public string MesDataImage1 { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public int Settle { get; set; }

    }
}
