using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web
{
    public class RowDataEntitiy
    {
        public string MachineType { get; set; }
        public string MachineId { get; set; }
        public string Rfid { get; set; }
        public string MoldName { get; set; }
        public string CycleNo { get; set; }
        public string ChGroup { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public string LimitValue { get; set; }
        public string CollectDate { get; set; }
        public string CollectDt { get; set; }

        public static RowItem EntityToItem(RowDataEntitiy _rowDataEntity1)
        {
            RowItem result = new RowItem();
            var props = typeof(RowItem).GetProperties();
            if (props.Any(x => x.Name == _rowDataEntity1.DataType))
            {
                var prop = props.First(x => x.Name == _rowDataEntity1.DataType);
                prop.SetValue(result, _rowDataEntity1.Value);
            }
            result.MachineId = _rowDataEntity1.MachineId;
            result.MachineType = _rowDataEntity1.MachineType;
            result.Rfid = _rowDataEntity1.Rfid;
            result.MoldName = _rowDataEntity1.MoldName;
            result.CollectDt = _rowDataEntity1.CollectDt;
            result.CycleNo = _rowDataEntity1.CycleNo;
            result.ChName = _rowDataEntity1.ChGroup;

            return result;
        }
    }

    public class RowItem
    {
        public string CycleNo { get; set; }
        public string ChName { get; set; }
        public string MachineId { get; set; }
        public string Rfid { get; set; }
        public string MoldName { get; set; }
        public string MachineType { get; set; }
        public string CollectDt { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Maximum_Temperature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Maximum_Pressure { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Mold_Temperature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Integral_Temperature { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Integral_Pressure { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Meltfront_Temperature { get; set; }

        public static RowItem DeepClone(RowItem originalItem)
        {
            RowItem rowItem = new RowItem();

            var props = typeof(RowItem).GetProperties();
            foreach (var prop in props)
            {
                prop.SetValue(rowItem, prop.GetValue(originalItem));
            }

            return rowItem;
        }
    }

    public class RowDataList : List<RowDataEntitiy>
    {
        public RowDataList(IEnumerable<RowDataEntitiy> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
