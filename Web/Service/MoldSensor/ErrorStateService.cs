using Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class ErrorStateService : BaseService, Map.IMap
    {
        #region Cache

        public static ErrorList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
               //Json 형식으로 값 전달
               // { "corpCode", corpCode }
            };

            return new ErrorList(DataContext.StringEntityList<ErrorEntity>("@Mold.MoldList", param));
        }

        public static ErrorList ListAllCache(string corpCode)
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

        public static ErrorList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static ErrorList Select()
        {
            HybridDictionary param = new HybridDictionary();
            List<ErrorEntity> list = DataContext.StringEntityList<ErrorEntity>("@Mold.MoldSelect", param);
            return new ErrorList(list);
        }

        public static ErrorList SelectOne(string corpCode, string rackNumber)
        {
            HybridDictionary param = new HybridDictionary();
            List<ErrorEntity> list = DataContext.StringEntityList<ErrorEntity>("@Rack.RackSelect", param);
            return new ErrorList(list);
        }

        public static List<ErrorEntity> GetErrorSearch(HybridDictionary param)
        {
            List<ErrorEntity> lstResultS = new List<ErrorEntity>();

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

            if (bIsContainMidNight)
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.Date.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                lstResultS.AddRange(DataContext.StringEntityList<ErrorEntity>("@Mold.SelectError", queryParam));

                queryParam["startDt"] = dtStart.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                lstResultS.AddRange(DataContext.StringEntityList<ErrorEntity>("@Mold.SelectError", queryParam));
            }
            else
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                lstResultS.AddRange(DataContext.StringEntityList<ErrorEntity>("@Mold.SelectError", queryParam));
            }


            if (lstResultS == null || lstResultS.Count == 0) 
                return new List<ErrorEntity>();

            foreach (var data in lstResultS)
            {
                data.ErrorType = string.Empty;

                if (data.Message.ToLower().Contains("lower"))
                    data.ErrorType = data.DataType + "/LowerError";
                else if (data.Message.ToLower().Contains("upper"))
                    data.ErrorType = data.DataType + "/UpperError";
                else
                    data.ErrorType = data.DataType + "/UnknownError";
            }

            return lstResultS;
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            ErrorList molds = ListAllCache(corpCode);
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
