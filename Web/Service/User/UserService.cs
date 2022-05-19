using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;
using Framework;
using Microsoft.Extensions.Options;

namespace Web
{
    public class UserService : BaseService, Map.IMap
    {
        public static UserList List(string corpCode)
        {
            return new (DataContext.StringEntityList<UserEntity>("@User.UserList", corpCode));
        }

        public static UserEntity LoginSelect(IDictionary dic)
        {
            return DataContext.StringEntity<UserEntity>("@User.UserLoginSelect", dic);
        }

        public static int Insert(IDictionary param)
        {
            return DataContext.StringNonQuery("@User.UserInsert", param);
        }

        public static int Update(ParameterDictionary param)
        {
            return DataContext.StringNonQuery("@User.UserUpdate", param);
        }

        public static int Delete(HybridDictionary param)
        {
            int result = 0;
            result += DataContext.StringNonQuery("@User.UserDelete", param);
            return result;
        }

        public static Map GetMap(string corpCode, string category = null)
        {
            return List(corpCode).AsEnumerable().Select(user=>
            {
                return new MapEntity(user.UserId, user.UserNm, category, 'Y');
            }).ToMap();
        }

        Map Map.IMap.GetMap(string corpCode, string category)
        {
            return GetMap(corpCode, category);
        }

        public static void RefreshMap(string corpCode)
        {
        }
        void Map.IMap.RefreshMap(string corpCode)
        {
            RefreshMap(corpCode);
        }
    }
}
