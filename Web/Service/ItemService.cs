using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using Framework;

namespace Web
{
    public class ItemService : BaseService, Map.IMap
    {
        public static ItemList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary() 
            {
                { "corpCode", corpCode }
            };

            return new ItemList(DataContext.StringEntityList<ItemEntity>("@Item.ItemList", param));
        }

        public static ItemList ListAllCache(string corpCode)
        {
          
            var list = UtilEx.FromCache(
                BuildCacheKey(corpCode), 
                DateTime.Now.AddMinutes(GetCacheMin()),
                ListAll,
                corpCode
             );
            
            return list;
        }

        public static void RemoveCache(string corpCode)
        {
            UtilEx.RemoveCache(BuildCacheKey(corpCode));
        }

        public static ItemList List(string corpCode)
        {
            return ListAll(corpCode);
        }


        public static ItemEntity Select(string corpCode, string itemCode)
        {
            return ListAllCache(corpCode).FirstOrDefault(x => x.ItemCode == itemCode);
        }

        public static string ItemName(string corpCode, string itemCode)
        {
            return Select(corpCode, itemCode).ItemName;
        }

        public static int ItemInsert(ItemEntity entity)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemInsert", entity);

            RemoveCache(entity.CorpCode);

            return rtn;
        }

        public static int ItemInsert(ParameterDictionary param)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemInsert", param);

            RemoveCache(param.TypeKey<string>("CorpCode"));

            return rtn;
        }

        public static int ItemInsert(string corpCode, string itemCode, string itemName, string carCode, string endPno, string subPno, string pno, DateTime startDt, DateTime endDt, char excelYn, string remark, string createUserId)
        {
            ParameterDictionary param = new ParameterDictionary() 
            {
                { "corpCode", corpCode }
            ,   { "itemCode", itemCode }
            ,   { "itemName", itemName }
            ,   { "carCode", carCode }
            ,   { "endPno", endPno }
            ,   { "subPno", subPno }
            ,   { "remark", remark }
            ,   { "createUserId", createUserId }
            };

            return ItemInsert(param);
        }

        public static int ItemInsert(IDictionary param)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemInsert", param);
            RemoveCache(param.TypeKey<string>("corpCode"));
            return rtn;
        }

        public static int ItemUpdate(ItemEntity entity)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemUpdate", entity);

            RemoveCache(entity.CorpCode);

            return rtn;
        }

        public static int ItemUpdate(ParameterDictionary param)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemUpdate", param);

            RemoveCache(param.TypeKey<string>("CorpCode"));

            return rtn;
        }

        public static int ItemUpdate(string corpCode, string itemCode, string itemName, string carCode, string endPno, string subPno, string pno, DateTime startDt, DateTime endDt, char excelYn, string remark, string updateUserId)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
                { "corpCode", corpCode }
            ,   { "itemCode", itemCode }
            ,   { "temName", itemName }
            ,   { "carCode", carCode }
            ,   { "endPno", endPno }
            ,   { "subPno", subPno }
            ,   { "pno", pno }
            ,   { "startDt", startDt }
            ,   { "endDt", endDt }
            ,   { "excelYn", excelYn }
            ,   { "remark", remark }
            ,   { "updateUserId", updateUserId }
            };

            return ItemUpdate(param);
        }

        public static int ItemUpdate(HybridDictionary param)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemUpdate", param);

            RemoveCache(param.TypeKey<string>("corpCode"));

            return rtn;
        }

        public static int ItemDelete(ItemEntity entity)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemDelete", entity);

            RemoveCache(entity.CorpCode);

            return rtn;
        }

        public static int ItemDelete(IDictionary param)
        {
            var rtn = DataContext.StringNonQuery("@Item.ItemDelete", param);

            RemoveCache(param.TypeKey<string>("corpCode"));

            return rtn;
        }

        public static int ItemDelete(string corpCode, string itemCode)
        {
            // 파라미터가 제거되어 전체 테이블이 삭제되는걸 방지하기위해 삭제시에는 에외적으로 ParameterDictionary 사용 안함
            HybridDictionary param = new HybridDictionary()
            {
                { "corpCode", corpCode }
            ,   { "itemCode", itemCode }
            };

            return ItemDelete(param);
        }

        public static Map GetMap(string corpCode, string category = null)
        {
            return ListAllCache(corpCode).Select(y => {
                return new MapEntity(y.ItemCode, y.ItemName, category, 'Y');
            }).ToMap();
        }

        Map Map.IMap.GetMap(string corpCode, string category)
        {
            return ItemService.GetMap(corpCode, category);
        }

        public static void RefreshMap(string corpCode)
        {
            RemoveCache(corpCode);
        }

        void Map.IMap.RefreshMap(string corpCode)
        {
            ItemService.RefreshMap(corpCode);
        }
    }
}
