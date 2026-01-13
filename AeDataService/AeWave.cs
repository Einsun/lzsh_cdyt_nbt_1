using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeDataService
{
    internal class AeWave
    {
        public int ID { get; set; }
        public string SN { get; set; } = "";
        public string Path { get; set; } = "";
        public int DBId { get; set; }
        public int DeviceId { get; set; }
        public DateTime Time { get; set; }
    }
}
