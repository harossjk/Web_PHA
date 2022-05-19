using Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class MoldLocationService : BaseService, Map.IMap
    {
        #region Cache

        public static MoldList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
               //Json 형식으로 값 전달
               // { "corpCode", corpCode }
            };

            return new MoldList(DataContext.StringEntityList<MoldEntity>("@Mold.MoldList", param));
        }

        public static MoldList ListAllCache(string corpCode)
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

        public static MoldList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static MoldList Select()
        {
            HybridDictionary param = new HybridDictionary();
            List<MoldEntity> list = DataContext.StringEntityList<MoldEntity>("@Mold.MoldSelect", param);
            return new MoldList(list);
        }

        public static MoldList SelectOne(string corpCode, string rackNumber)
        {
            HybridDictionary param = new HybridDictionary();
            List<MoldEntity> list = DataContext.StringEntityList<MoldEntity>("@Rack.RackSelect", param);
            return new MoldList(list);
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            MoldList molds = ListAllCache(corpCode);
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
