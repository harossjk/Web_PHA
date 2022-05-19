using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class TempEntitiy
    {
        public string Name { get; set; }  
    }

    public class TempList : List<TempEntitiy>
    {
        public TempList(IEnumerable<TempEntitiy> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
