using Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class TempService : BaseService, Map.IMap
    {
        #region Cache

        public static TempList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
               //Json 형식으로 값 전달
               // { "corpCode", corpCode }
            };

            return new TempList(DataContext.StringEntityList<TempEntitiy>("@Mold.MoldList", param));
        }

        public static TempList ListAllCache(string corpCode)
        {
            var list = UtilEx.FromCache(
                "MOLD",
                DateTime.Now.AddMinutes(GetCacheMin()),
                ListAll,
                corpCode
             );

            return list;
        }

        public static void RemoveCache(string corpCode)
        {
            UtilEx.RemoveCache("MOLD");
        }

        public static TempList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static TempList Select()
        {
            HybridDictionary param = new HybridDictionary();
            List<TempEntitiy> list = DataContext.StringEntityList<TempEntitiy>("@Mold.MoldSelect", param);
            return new TempList(list);
        }

        public static TempList SelectOne(string corpCode, string rackNumber)
        {
            HybridDictionary param = new HybridDictionary();
            List<TempEntitiy> list = DataContext.StringEntityList<TempEntitiy>("@Rack.RackSelect", param);
            return new TempList(list);
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            TempList molds = ListAllCache(corpCode);
            return ListAll(corpCode).AsEnumerable().Select(y =>
            {
                return new MapEntity("", "", category, 'Y');
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
        #endregion
    }
}
