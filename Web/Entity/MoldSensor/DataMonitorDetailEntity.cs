using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class DataMonitorDetailEntity
    {
        public string CycleNo { get; set; }
        public string MachineId { get; set; }
        public string MachineType { get; set; }
        public string ChName { get; set; }
        public string ChGroup { get; set; }
        public string DataType { get; set; }
    }

    public class MonitorDetailList : List<DataMonitorDetailEntity>
    {
        public MonitorDetailList(IEnumerable<DataMonitorDetailEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
