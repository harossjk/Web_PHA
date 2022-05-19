using System;
using System.Collections.Generic;

using Framework;

namespace Web
{
    public class MapEntity : BaseEntity
    {
        public MapEntity (string value, string label, string parent, char useYn)
        {
            Value = value;
            Label = label;
            Parent = parent;
            UseYn = useYn;
        }

        public MapEntity(string value, string label, int jego, string parent, char useYn)
        {
            Value = value;
            Label = label;
            Jego = jego;
            Parent = parent;
            UseYn = useYn;
        }

        public string Value { get; set; }
        public string Label { get; set; }
        public int Jego { get; set; }
        public string Parent { get; set; }
        public char UseYn { get; set; }

        public override string ToString()
        {
            return $"[{Value}] {Label}";
        }


    }

    public class MapList : List<MapEntity>
    {
        public MapList(IEnumerable<MapEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
