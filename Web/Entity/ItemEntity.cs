using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework;

namespace Web
{
    public class ItemEntity : BaseEntity
    {

        public string CorpCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CarCode { get; set; }
        public string EndPno { get; set; }
        public string SubPno { get; set; }
        public string Remark { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDt { get; set; }

        public override string ToString()
        {
            return $"{ItemCode},{ItemName},{EndPno},{SubPno}";
        }
    }

    public class ItemList : List<ItemEntity>
    {
        private IEnumerable<CarEntity> enumerable;

        public ItemList(IEnumerable<ItemEntity> list) : base(list)
        {
        }

        public ItemList(IEnumerable<CarEntity> enumerable)
        {
            this.enumerable = enumerable;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
