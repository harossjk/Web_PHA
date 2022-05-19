using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Data;
using Framework;
using Microsoft.AspNetCore.Http;

namespace Web.Controllers
{
    public class ControllerBaseEx : ControllerBase
    {
        static readonly string _corpCodeKey = "corpCode";
        static readonly string _createUserIdKey = "createUserId";
        static readonly string _updateUserIdKey = "updateUserId";

        public string UserCorpCode
        {
            get
            {
                return HttpContext.Items.SafeTypeKey("CorpCode", "PH"); //TODO: 추후 기본값 삭제
            }
        }


        public string UserId
        {
            get
            {
                return HttpContext.Items.SafeTypeKey("UserId", "admin"); //TODO: 추후 기본값 삭제
            }
        }

        protected void RefineParam(IEnumerable<IDictionary> list, bool isRemoveNull = true)
        {
            foreach (var dic in list)
            {
                RefineParam(dic);
            }
        }

        protected void RefineParam(IDictionary dic, bool isRemoveNuill = true)
        {
            if (dic[_corpCodeKey] == null || string.IsNullOrWhiteSpace(dic[_corpCodeKey].ToString()))
                dic[_corpCodeKey] = UserCorpCode;

            dic[_createUserIdKey] = UserId;
            dic[_updateUserIdKey] = UserId;

            if (!isRemoveNuill)
                return;

            string[] keys = new string[dic.Keys.Count];

            dic.Keys.CopyTo(keys, 0);

            for (int i = dic.Keys.Count - 1; i >= 0; i--)
            {
                string key = keys[i];
                if (dic[key] == null)
                    dic.Remove(key);
            }
        }

        protected IDictionary<string, IEnumerable<IDictionary>> ToDic(DataSet ds)
        {
            var dic = new Dictionary<string, IEnumerable<IDictionary>>();

            foreach (DataTable dt in ds.Tables)
                dic.Add(dt.TableName, ToDic(dt));

            return dic;
        }

        protected IEnumerable<IDictionary> ToDic(DataTable dt)
        {
            return dt.ToDic(UtilEx.ToCamel);
        }

        protected XDocument ToXDoc<T>(IEnumerable<T> list, string rootName = "root", string itemName = "item")
        {
            IDictionary root = new HybridDictionary() {
                { rootName, new HybridDictionary() {
                    { itemName, list } } 
                }
            };

            string json = JsonConvert.SerializeObject(root);

            return JsonConvert.DeserializeXNode(json);
        }

        protected XDocument ToXDoc(IDictionary dic, string rootName = "root", string itemName = "item")
        {
            IDictionary root = new HybridDictionary() {
                { rootName, new HybridDictionary() {
                    { itemName, dic } }
                }
            };

            string json = JsonConvert.SerializeObject(root);

            return JsonConvert.DeserializeXNode(json);
        }
        
        public string NewGuid()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }

        public byte[] ToBinary(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
