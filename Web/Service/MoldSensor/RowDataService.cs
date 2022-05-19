using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web
{
    public class RowDataService : BaseService, Map.IMap
    {
        #region Cache

        public static RowDataList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
               //Json 형식으로 값 전달
               // { "corpCode", corpCode }
            };

            return new RowDataList(DataContext.StringEntityList<RowDataEntitiy>("@Mold.MoldList", param));
        }

        public static RowDataList ListAllCache(string corpCode)
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

        public static RowDataList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static List<RowItem> Select(HybridDictionary param)
        {
            List<RowItem> lstResultS = new List<RowItem>();

            // ShiftType (=workType)을 기준으로 시작날짜의 시간과 종료날짜의 시간 선언
            string sShiftType = param["workType"].ToString();
            DateTime dtStart = Convert.ToDateTime(param["startDt"].ToString());
            DateTime dtEnd = Convert.ToDateTime(param["endDt"].ToString());
            bool bIsContainMidNight = Utility.GetSchedule(sShiftType, ref dtStart, ref dtEnd);
            // -----------------------------------------------------------------------

            HybridDictionary queryParam = new HybridDictionary()
                {
                    { "rfid", Convert.ToInt32(param["rfid"]).ToString() },
                    { "startDt", ""},
                    { "endDt", ""},
                };

            List<RowDataEntitiy> lstPressure = new List<RowDataEntitiy>();
            List<RowDataEntitiy> lstTemperature = new List<RowDataEntitiy>();

            if (bIsContainMidNight)
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.Date.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                lstPressure.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectPressureList", queryParam));
                lstTemperature.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectTemperatureList", queryParam));

                queryParam["startDt"] = dtStart.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");

                lstPressure.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectPressureList", queryParam));
                lstTemperature.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectTemperatureList", queryParam));
            }
            else
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                lstPressure.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectPressureList", queryParam));
                lstTemperature.AddRange(DataContext.StringEntityList<RowDataEntitiy>("@Raw.SelectTemperatureList", queryParam));
            }


            if (lstPressure == null || lstTemperature == null || (lstPressure.Count == 0 && lstTemperature.Count == 0))
                return new List<RowItem>();

            lstPressure.ForEach(x => x.CollectDate = Utility.GetDate(DateTime.Parse(x.CollectDt)).ToString("yyyy-MM-dd"));
            lstTemperature.ForEach(x => x.CollectDate = Utility.GetDate(DateTime.Parse(x.CollectDt)).ToString("yyyy-MM-dd"));

            lstPressure
                .GroupBy(x => new { x.Rfid, x.CycleNo, x.MachineType, x.ChGroup, x.CollectDate })
                .OrderBy(x => x.Key.CollectDate)
                .ToDictionary(x => x.Key, y => y.Select(x => RowDataEntitiy.EntityToItem(x)).ToList())
                .Values
                .ToList()
                .ForEach(x => lstResultS.Add(CombineDataTypeValue(x)))
                ;

            lstTemperature
                .GroupBy(x => new { x.Rfid, x.CycleNo, x.MachineType, x.ChGroup, x.CollectDate })
                .OrderBy(x => x.Key.CollectDate)
                .ToDictionary(x => x.Key, y => y.Select(x => RowDataEntitiy.EntityToItem(x)).ToList())
                .Values
                .ToList()
                .ForEach(x => lstResultS.Add(CombineDataTypeValue(x)))
                ;

            return lstResultS;
        }

        public static RowItem CombineDataTypeValue(List<RowItem> rowItemList)
        {
            RowItem result = new RowItem();

            if (rowItemList == null || rowItemList.Count == 0)
                return result;

            result = RowItem.DeepClone(rowItemList.OrderBy(x => DateTime.Parse(x.CollectDt)).First());

            foreach (var rowItem in rowItemList)
            {
                var props = typeof(RowItem).GetProperties();
                foreach (var prop in props)
                {
                    if (prop.GetValue(rowItem) != null)
                        prop.SetValue(result, prop.GetValue(rowItem));
                }
            }

            return result;
        }

        public static RowDataList SelectOne(string corpCode, string rackNumber)
        {
            HybridDictionary param = new HybridDictionary();
            List<RowDataEntitiy> list = DataContext.StringEntityList<RowDataEntitiy>("@Rack.RackSelect", param);
            return new RowDataList(list);
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            RowDataList molds = ListAllCache(corpCode);
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
