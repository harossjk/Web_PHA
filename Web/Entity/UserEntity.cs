using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Framework;
using Newtonsoft.Json;

namespace Web
{    
    public class UserEntity : BaseEntity
    {
        public string CorpCode { get; set; }
        public string UserId { get; set; }
        public string PartnerCode { get; set; }
        public string UserNm { get; set; }
        [JsonIgnore]
        public string UserPw { get; set; }
        public string UserDept { get; set; }
        public string UserGubun { get; set; }
        public DateTime LoginDt { get; set; }
        public DateTime LogoutDt { get; set; }
        public string UseYn { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateDt { get; set; }
        public string UpdateUserId { get; set; }
        public DateTime UpdateDt { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string MenuAuth { get; set; }

        [JsonIgnore]
        public Dictionary<string, int> _menuAuthDic;        
        public Dictionary<string, int> MenuAuthDic
        {
            get
            {
                if (_menuAuthDic == null)
                {
                    Dictionary<string, int> rtn = new Dictionary<string, int>();

                    var xDoc = ToXml(MenuAuth);
                    if (xDoc.IsEmpty())
                        return rtn;

                    _menuAuthDic = xDoc.Root.Elements().ToDictionary(
                        x => x.Element("menuId").Value,
                        x => x.Element("auth").TypeVal<int>(0));
                }

                return _menuAuthDic;
            }
            set
            {
                _menuAuthDic = value;

                XDocument xDoc = new XDocument(new XElement("root"));
                
                value.Where(x => x.Value > 0).ToList().ForEach(item => {
                    xDoc.Root.Add(
                        new XElement("menuId", item.Key),
                        new XElement("auth", item.Value));
                });

                MenuAuth = xDoc.ToString();
            }
        }

        public override string ToString()
        {
            return $"{UserId}, {UserNm}, {Token}";
        }
    }
    public class UserList : List<UserEntity>
    {
        public UserList(IEnumerable<UserEntity> list) : base(list) { }
        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }


}


