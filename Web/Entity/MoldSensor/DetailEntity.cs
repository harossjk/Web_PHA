using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web
{
    public class DetailEntity
    {
        public string CycleNo { get; set; }
        public string MachineId { get; set; }
        public string MachineType { get; set; }
        public string ChName { get; set; }
        public string Value { get; set; }
        public string ChGroup { get; set; }
        public string DataType { get; set; }
        public string Color { get; set; }
    }

    public class DetailCustomEntitiy
    {
        public string CycleNo { get; set; }
        public List<DetailItem> Values { get; set; }
    }

    public class DetailItem
    {
        public string ChName { get; set; }
        public string MachineId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Color { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MaxTemperature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MaxPressure { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MoldTemp { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IntergralTemperature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IntergralPressure { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MeltFront { get; set; }
    }

    public class AlarmEntitiy
    {
        public string Rfid { get; set; }
        public string MoldName { get; set; }
        public string MachineId { get; set; }
        public string DataType { get; set; }
        public string Message { get; set; }
        public string ChName { get; set; }
        public DateTime CollectDt { get; set; }
        public string Value { get; set; }
        public string LimitValue { get; set; }

    }


    public class MonitorDetailCustomList : List<DetailCustomEntitiy>
    {
        public MonitorDetailCustomList(IEnumerable<DetailCustomEntitiy> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }


}
 