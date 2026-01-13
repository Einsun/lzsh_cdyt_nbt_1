namespace NBWebApp.Models
{
    /// <summary>
    /// 结果信息
    /// </summary>
    public class ResultInfo
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 个数
        /// </summary>
        public int Count{get;set;}
    }
}
