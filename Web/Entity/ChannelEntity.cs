using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework;

namespace Web
{
    public class ChannelEntity  
    {
        public string ChName { get; set; }
    }

    public class ChannelList : List<ChannelEntity>
    {
        public ChannelList()
        {
        }
        public ChannelList(IEnumerable<ChannelEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
