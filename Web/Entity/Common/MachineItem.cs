using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class MachineItem
    {
        public string Rfid { get; set; }
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }

        public Dictionary<string, List<DataInpItem>> ChannelS { get; set; }
    }
}
