using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework;

namespace Web
{
    public class CarEntity : BaseEntity
    {
        public string CorpCode { get; set; }
        public string CarCode { get; set; }
        public string CarName { get; set; }
        public string ProjectName { get; set; }
        public string AssetGubun { get; set; }
        public char UseYn { get; set; }
        public string Remark { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDt { get; set; }
        public string CodeName { get; set; }

        public override string ToString()
        {
            return $"{CarCode},{CarName},{ProjectName}";
        }
    }

    public class CarList : List<CarEntity>
    {
        public CarList(IEnumerable<CarEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
