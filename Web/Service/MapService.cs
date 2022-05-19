using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using Framework;
using Newtonsoft.Json;

namespace Web
{
    public class Map : List<MapEntity>
    {
        public interface IMap
        {
            public Map GetMap(string corpCode, string category = null);

            public void RefreshMap(string corpCode);
        }

        
        public class MapInfra : Tuple<Func<string, string, Map>, Action<string>>
        {
            public MapInfra(Func<string, string, Map> func, Action<string> act) : base(func, act)
            {
            }
        }

        public Map(List<MapEntity> map) : base(map)
        {
        }        
    }

    public static class MapExtension
    {
        public static Map ToMap(this IEnumerable<MapEntity> list)
        {
            return new Map(list.ToList());
        }
    }

    public class MapService : BaseService
    {
        static IDictionary<string, Map.MapInfra> _mapList = 
            new Dictionary<string, Map.MapInfra>()
        {
            //{ "code",           new Map.MapInfra(CodeService.GetMap, CodeService.RefreshMap) },
            //{ "car",            new Map.MapInfra(CarService.GetMap, CarService.RefreshMap) },
            //{ "item",           new Map.MapInfra(ItemService.GetMap, ItemService.RefreshMap) },
            //{ "checksheet",     new Map.MapInfra(CheckStdMgtService.GetMap, CheckStdMgtService.RefreshMap) },
            //{ "mold",           new Map.MapInfra(MoldService.GetMap, MoldService.RefreshMap) },
            //{ "partner",        new Map.MapInfra(PartnerService.GetMap, PartnerService.RefreshMap) },
            //{ "testbed",        new Map.MapInfra(TestbedService.GetMap, TestbedService.RefreshMap) },
            //{ "user",           new Map.MapInfra(UserService.GetMap, UserService.RefreshMap) },
            //{ "spare",           new Map.MapInfra(SpareMasterService.GetMap, SpareMasterService.RefreshMap) },
        };

        public static List<MapEntity> GetList(string corpCode, string mapCode, string category)
        {
            if (!_mapList.ContainsKey(mapCode))
                return new List<MapEntity>();

            return _mapList[mapCode].Item1.Invoke(corpCode, category);
        }


        public static void RefreshMap(string corpCode, string mapCode)
        {
            _mapList[mapCode].Item2.Invoke(corpCode);
        }



        //1213 seo
        public static List<MapEntity> GetSubList(string corpCode, string mapCode, string subCategory)
        {
            if (!_mapList2.ContainsKey(mapCode))
                return new List<MapEntity>();

            return _mapList2[mapCode].Item1.Invoke(corpCode, subCategory);
        }

        static IDictionary<string, Map.MapInfra> _mapList2 =
         new Dictionary<string, Map.MapInfra>()
     {
            //{ "code",           new Map.MapInfra(CodeService.GetMap2, CodeService.RefreshMap) },
     };
    }
}
