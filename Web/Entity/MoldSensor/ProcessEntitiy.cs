using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class ProcessEntitiy
    {
        public List<ProcessStateRfidItem> RfidItemList { get; set;}
    }

    public class ProcessList : List<ProcessEntitiy>
    {
        public ProcessList(IEnumerable<ProcessEntitiy> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }

    public class ProcessStateItem
    {
        public int Index { get; set; }
        public string MachineId { get; set; }
        public string CycleNo { get; set; }
        public string Inp { get; set; }
        public DateTime CollectDt { get; set; }
    }

    public class ProcessStateChannelItem
    {
        // 하한 설정값
        public string LowerLimit { get; set; }
        // 상한 설정값
        public string UpperLimit { get; set; }
        // 해당 채널의 데이터
        public string Average { get; set; }
        // 표준편차
        public string StandardDeviation { get; set; }
        // 분산
        public string VarianceValue { get; set; }
        // 데이터 평균
        public List<ProcessStateItem> TotalData { get; set; }
        // 하한에러 데이터 Index key
        public List<int> LowerErrorDataKeyS { get; set; }
        // 상한에러 데이터 Index Key
        public List<int> UpperErrorDataKeyS { get; set; }
        // 하한에러 Top5 Index key
        public List<int> LowerTop5DataKeyS { get; set; }
        // 상한에러 Top5 Index Key
        public List<int> UpperTop5DataKeyS { get; set; }
    }

    public class ProcessStateRfidItem
    {
        // Rfid
        public string Rfid { get; set; }
        public Dictionary<string, ProcessStateChannelItem> ChannelItemDic { get; set; }
    }
}
