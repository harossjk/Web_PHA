using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using ProtoBuf;

using Framework;

namespace Web
{
    public enum PermMethod
    {
        Read = 0
    ,   Create
    ,   Update
    ,   Delete
    ,   Admin
    ,   Etc1
    ,   Etc2
    ,   Etc3
    ,   Etc4
    ,   Etc5
    ,   Void
    }

    [ProtoContract]
    public class MenuEntity : BaseEntity
    {
        public MenuEntity()
        {
        }

        [ProtoMember(1)]
        public string MenuId { get; set; }
        [ProtoMember(2)]
        public string MenuName { get; set; }
        [ProtoMember(3)]
        public char MenuType { get; set; }
        [ProtoMember(4)]
        public string ParentId { get; set; }
        [ProtoMember(5)]
        public char UseYn { get; set; }
        [ProtoMember(6)]
        public int DisplayOrder { get; set; }
        [ProtoMember(7)]
        public string MenuBody { get; set; }
        [JsonIgnore]
        public string CreateUserId { get; set; }
        [JsonIgnore]
        public DateTime CreateDt { get; set; }
        [JsonIgnore]
        public string UpdateUserId { get; set; }
        [JsonIgnore]
        public DateTime UpdateDt { get; set; }

        public override string ToString()
        {
            return $"[{MenuId}:{MenuType}] {MenuName}";
        }
    }

    [ProtoContract]
    public class MenuList : List<MenuEntity>
    {
        public MenuList()
        {
        }

        public MenuList(IEnumerable<MenuEntity> list) : base(list)
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}
