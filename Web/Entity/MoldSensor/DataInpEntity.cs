using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class DataInpEntity
    {
        public string Rfid { get; set; }
        public string CycleNo { get; set; }
        public string MachineType { get; set; }
        public string MachineId { get; set; }
        public string Value { get; set; }
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }
        public DateTime CollectDt { get; set; }

        public static ProcessStateItem EntityToItem(DataInpEntity _dataInpEntity)
        {
            if (_dataInpEntity.LowerLimit.Contains("(null)"))
                _dataInpEntity.LowerLimit = "NaN";

            if (_dataInpEntity.UpperLimit.Contains("(null)"))
                _dataInpEntity.UpperLimit = "NaN";
            
            ProcessStateItem proitem = new ProcessStateItem();
            proitem.MachineId = _dataInpEntity.MachineId;
            proitem.CollectDt = _dataInpEntity.CollectDt;
            proitem.CycleNo = _dataInpEntity.CycleNo;

            if (_dataInpEntity.Value == null || _dataInpEntity.Value.Contains("(null)"))
                proitem.Inp = "NaN";
            else
                proitem.Inp = Math.Round(Convert.ToDouble(_dataInpEntity.Value), 4).ToString();

            //return new ProcessStateItem()
            //{
            //    MachineId = _dataInpEntity.MachineId,
            //    CollectDt = _dataInpEntity.CollectDt,
            //    CycleNo = _dataInpEntity.CycleNo,
            //    Inp = Math.Round(Convert.ToDouble(_dataInpEntity.Value), 4).ToString()
            //};
            return proitem;
        }
    }

    public class DataInpCustomEntitiy
    {
        public string MachineType { get; set; }
        public List<string> MachineId { get; set; }
        public string LowerLimit { get; set; }
        public string UpperLimit { get; set; }
        public List<DataInpItem> Values { get; set; }
    }

    public class DataInpItem
    {
        public string CycleNo;
        public string Inp;
        public DateTime CollectDt;
        public string MachineId;
    }

    public class MonitorInplList : List<DataInpCustomEntitiy>
    {
        public MonitorInplList()
        {
        }
        public MonitorInplList(IEnumerable<DataInpCustomEntitiy> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }

    public class DataInpSearchEntity: DataInpEntity
    {
        public string ChName  { get; set; }
    }

    public class DataInpSearch
    {
        public Dictionary<string, MachineItem> SearchInpItemS { get; set; }
    }

    public class MonitorInpSearchlList : List<DataInpSearch>
    {
        public MonitorInpSearchlList()
        {
        }
        public MonitorInpSearchlList(IEnumerable<DataInpSearch> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}

