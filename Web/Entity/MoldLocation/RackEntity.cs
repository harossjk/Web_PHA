using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class RackList : List<RackEntity>
    {
        public RackList(IEnumerable<RackEntity> list) : base(list) { }
        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }

    public class RackEntity : BaseEntity
    {
        public string CorpCode { get; set; }
        public string FactoryCode { get; set; }
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        public string LocationGubun { get; set; }
        public string RackNumber { get; set; }
        public string Barcode { get; set; }
        public string Floor { get; set; }
    }

    public class MoldPosEntity
    {
        public string CorpCode { get; set; }
        public string FactoryCode { get; set; }
        public string Rfid { get; set; }
        public string Gubun { get; set; }
        public string Position { get; set; }
        public string LocationName { get; set; }
        public string LocationBarcode { get; set; }
        public string RackBarcode { get; set; }
        public string MoldName { get; set; }
        public string Cavity { get; set; }
        public string Ton { get; set; }
    }


    public class MachineEntity
    {
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public string MachineType { get; set; }
        public string MachineLegend{ get; set; }
        public string Status { get; set; }
        public string ViewStatus { get; set; }
        public string date { get; set; }
    }

    public class LocationEntity
    {
        public string LocationName { get; set; }
        public string Barcode { get; set; }
        public string LocationGubun { get; set; }
        public string LocationLegend { get; set; }

    }

}
