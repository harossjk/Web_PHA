using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class ErrorEntity
    {
        public string Rfid { get; set; }
        public string MachineId { get; set; }
        public string MoldName { get; set; }
        public string DataType { get; set; }
        public string Message { get; set; }
        public string ChGroup { get; set; }
        public DateTime CollectDt { get; set; }
        public string Value { get; set; } 
        public string LimitValue { get; set; }
        public string ErrorType { get; set; }
    }

    public class ErrorList : List<ErrorEntity>
    {
        public ErrorList(IEnumerable<ErrorEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
