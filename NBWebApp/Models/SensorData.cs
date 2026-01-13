using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace NBWebApp.Models
{
    /// <summary>
    /// 传感器数据
    /// </summary>
    public class SensorData
    {
        /// <summary>
        /// KEY
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 传输设备ID
        /// </summary>
        public int CommDeviceId { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; } = "";
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
