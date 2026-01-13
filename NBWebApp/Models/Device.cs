using Microsoft.CodeAnalysis.CSharp;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace NBWebApp.Models
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Device
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
        /// 设备编号
        /// </summary>
        public string SN { get; set; } = "";
        /// <summary>
        /// 设备类型
        /// </summary>
        public GatherType Type { get; set; }
        /// <summary>
        /// 产线ID
        /// </summary>
        public int ProductLineId { get; set; }
        /// <summary>
        /// 传输设备ID
        /// </summary>
        public int CommDeviceId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public int Address { get; set; }
        /// <summary>
        /// 通道
        /// </summary>
        public int Pos { get; set; }
        /// <summary>
        /// 报警规则ID
        /// </summary>
        public int? AlarmRuleId { get; set; }
        ///// <summary>
        ///// 采集规则ID
        ///// </summary>
        //public int? GatherRuleId { get; set; }
        ///// <summary>
        ///// 声发射相关信息
        ///// </summary>
        //public string AEInfo { get; set; } = "{}";
        /// <summary>
        /// 声发射规则ID
        /// </summary>
        public int? AeRuleId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        [Column(TypeName = "decimal(10,5)")]
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        [Column(TypeName = "decimal(10,5)")]
        public decimal MinValue { get; set; }
        /// <summary>
        /// 采样位
        /// </summary>
        public int Samping { get; set; }
        /// <summary>
        /// 信号类型
        /// </summary>
        public int SignalTypeId { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
    /// <summary>
    /// 声发射设备信息
    /// </summary>
    public class AEDevice
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
        /// 设备编号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 产线ID
        /// </summary>
        public int ProductLineId { get; set; }
        /// <summary>
        /// 传输设备ID
        /// </summary>
        public int CommDeviceId { get; set; }
        /// <summary>
        /// 声发射规则ID
        /// </summary>
        public int? AeRuleId { get; set; }
        ///// <summary>
        ///// 声发射相关信息
        ///// </summary>
        //public AeInfo? AeInfo { get; set; }
    }
    /// <summary>
    /// 声发射相关信息
    /// </summary>
    public class AeInfo
    {
        /// <summary>
        /// packet_name: 字符串，分为RAEM1/RAEM2/QCIOTGW
        /// </summary>
        public string packet_name { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 网关ID
        /// </summary>
        public string gateway_id { get; set; }
        ///// <summary>
        ///// 此条消息上报的时间，单位为秒数
        ///// </summary>
        //public UInt32 unix_time { get; set; }
        ///// <summary>
        ///// 此条消息上报的时间，单位为毫秒数
        ///// </summary>
        //public Int32 microsecond { get; set; }
        ///// <summary>
        ///// 错误状态上报
        ///// </summary>
        //public string? err_comment { get; set; }
        ///// <summary>
        ///// 系统软件版本
        ///// </summary>
        //public string? ver { get; set; }
    }
    /// <summary>
    /// 声发射采集设置
    /// </summary>
    public class AeMeasureConfig
    {
        /// <summary>
        /// 门限，单位为db
        /// </summary>
        public double ae_threshold { get; set; }
        /// <summary>
        /// 采集速率，单位为k/s
        /// </summary>
        public UInt32 ae_measure_speed { get; set; }
        /// <summary>
        /// 采集模式，1为包络采集，2为连续采集
        /// </summary>
        public UInt32 ae_measure_mode { get; set; }
        /// <summary>
        /// 波形截止时间，包络模式下有效，单位为us
        /// </summary>
        public UInt32 ae_eet { get; set; }
        /// <summary>
        /// 波形定义时间，包络模式下有效，单位为us
        /// </summary>
        public UInt32 ae_hdt { get; set; }
        /// <summary>
        /// 波形闭锁时间，包络模式下有效，单位为us
        /// </summary>
        public UInt32 ae_hlt { get; set; }
        /// <summary>
        /// 采样长度，单位为us，连续采集下有效
        /// </summary>
        public UInt32 ae_measure_length { get; set; }
        /// <summary>
        /// 采样次数，单位为次，连续采集下有效
        /// </summary>
        public UInt32 ae_measure_times { get; set; }
        /// <summary>
        /// 采样间隔，单位为us，连续采集下有效
        /// </summary>
        public UInt32 ae_measure_interval { get; set; }
        /// <summary>
        /// 参数发送使能，0/1
        /// </summary>
        public bool ae_para_enable { get; set; }
        /// <summary>
        /// 波形发送使能，0/1
        /// </summary>
        public bool ae_wave_enable { get; set; }
        /// <summary>
        /// 系统当前时间，单位为秒
        /// </summary>
        public UInt32 ae_system_time { get; set; }
        /// <summary>
        /// 系统采集状态，0为停止，1位启动
        /// </summary>
        public bool ae_measure_state { get; set; }
    }
    /// <summary>
    /// 声发射评级设置
    /// </summary>
    public class AeLevelConfig
    {
        /// <summary>
        /// 评级功能使能，0为禁止，1为使能
        /// </summary>
        public bool ae_level_enable { get; set; }
        ///// <summary>
        ///// 强度设置，为json格式
        ///// </summary>
        //public JsonObject? ae_level_strength { get; set; }
        ///// <summary>
        ///// 活度设置，为json格式
        ///// </summary>
        //public JsonObject? ae_level_activity { get; set; }
        /// <summary>
        /// 评级统计时间，单位为秒
        /// </summary>
        public UInt32 ae_level_cal_time { get; set; }
        /// <summary>
        /// 实时强度上报类型 0：不上报 1：上报1级强度 2：上报2级强度 3：上报3级强度
        /// </summary>
        public UInt32 ae_level_strength_report_type { get; set; }
        /// <summary>
        /// 实时强度上报最小间隔 秒为单位
        /// </summary>
        public UInt32 ae_level_strength_report_interval { get; set; }
    }
    /// <summary>
    /// 声发射定时设置
    /// </summary>
    public class AeTimingConfig
    {
        /// <summary>
        /// 无作用，都是1
        /// </summary>
        public bool ae_timing_enable { get; set; }
        /// <summary>
        /// 定时采集类型，1表示为连续采集模式，3表示为间隔采集模式
        /// </summary>
        public UInt32 ae_timing_type { get; set; }
        /// <summary>
        /// 定时采集时长，只有ae_timing_type为3时有效，单位为秒
        /// </summary>
        public UInt32 ae_timing_length { get; set; }
        /// <summary>
        /// 定时采集休眠时间，只有ae_timing_type为3是有效，单位为秒
        /// </summary>
        public UInt32 ae_timing_sleep { get; set; }
    }
}
