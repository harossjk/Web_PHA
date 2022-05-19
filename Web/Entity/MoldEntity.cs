using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class MoldEntity
    {
        public string Rfid { get; set; }
        public string MoldName { get; set; }
        public string Cavity { get; set; }
        public string Ton { get; set; }
    }

    public class MoldList : List<MoldEntity>
    {
        public MoldList(IEnumerable<MoldEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
