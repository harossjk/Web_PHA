using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class DataMonitorSelectEntity
    {
        public string MachineType { get; set; }
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public string MachineLegend { get; set; }
        public string ViewStatus { get; set; }
        public string CurMoldfileName { get; set; }
        public string SensorIsActive { get; set; }
    }
   

    public class MonitorSelectList : List<DataMonitorSelectEntity>
    {
        public MonitorSelectList(IEnumerable<DataMonitorSelectEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
