using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class DataMonitorEntity
    {
        public string MachineType { get; set; }
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public string ChName { get; set; }
        public string CycleNo { get; set; }
        public string Value { get; set; }
        public string Color { get; set; }
        public bool IsVisible { get; set; }
        public List<Dictionary<string, ColorState>> ColorS { get; set; }
        public List<dynamic> Data { get; set; }
        public string MachineLegend { get; set; }
        public string ViewStatus { get; set; }
        public string PUpperLimit { get; set; }
        public string PLowerLimit { get; set; }
        public string TUpperLimit { get; set; }
        public string TLowerLimit { get; set; }
        public string CurMoldfileName { get; set; }
        public string SensorIsActive { get; set; }
    }

    public class CMinMaxValue
    {
        public string ChName { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

    }

    public class ColorState
    {
        public string Color { get; set; }
        public bool Visible { get; set; }
    }


    public class MonitorList : List<DataMonitorEntity>
    {
        public MonitorList(IEnumerable<DataMonitorEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
