namespace NBWebApp.Models
{
    class DevInfo
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public List<DevInfo> Datas { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AeDevInfo : AEDevice
    {
        /// <summary>
        /// 
        /// </summary>
        public int TimingType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TimingLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TimingSleep { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int State { get; set; }
    }
}
