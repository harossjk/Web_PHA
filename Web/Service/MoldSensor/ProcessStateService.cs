using Framework;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class ProcessStateService : BaseService, Map.IMap
    {
        #region Cache

        public static ProcessList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
                //Json 형식으로 값 전달
                // { "corpCode", corpCode }
            };

            return new ProcessList(DataContext.StringEntityList<ProcessEntitiy>("@Mold.MoldList", param));
        }

        public static ProcessList ListAllCache(string corpCode)
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

        public static ProcessList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static MoldList Select()
        {
            HybridDictionary param = new HybridDictionary();
            List<MoldEntity> list = DataContext.StringEntityList<MoldEntity>("@Mold.SelectMold", param);
            return new MoldList(list);
        }

        public static ProcessList SelectOne(string corpCode, string rfid)
        {
            HybridDictionary param = new HybridDictionary();
            param.Add("corpCode", corpCode);
            param.Add("rfid", rfid);

            List<ProcessEntitiy> list = DataContext.StringEntityList<ProcessEntitiy>("@Rack.SelectMold", param);
            return new ProcessList(list);
        }

        public static ProcessEntitiy GetIntegralPressureSearch(HybridDictionary param)
        {
            // 결과 저장할 객체
            ProcessEntitiy Result = new ProcessEntitiy();
            if (Result.RfidItemList == null)
                Result.RfidItemList = new List<ProcessStateRfidItem>();

            // 설비 엔티티 리스트
            var lstMachineList = DataContext.StringEntityList<MachineEntity>("@Mold.SelectMachine", new HybridDictionary());
            if (lstMachineList == null || lstMachineList.Count == 0) return Result;

            // 설비 가공데이터 딕셔너리 (Key: MachineId, Value: ChannelList)
            Dictionary<string, List<string>> dicMachineIdChannel = new Dictionary<string, List<string>>();
            foreach (var MachineEntity in lstMachineList)
            {
                var lstChannelEntity = DataContext.StringEntityList<ChannelEntity>("@Mold.SelectOnePressureChList", new HybridDictionary() { { "machineId", MachineEntity.MachineId } });
                if (lstChannelEntity != null && lstChannelEntity.Count > 0)
                    dicMachineIdChannel.Add(MachineEntity.MachineId, lstChannelEntity.Select(x => x.ChName).ToList());
            }

            List<DataInpSearchEntity> lstTempSearchInpS = new List<DataInpSearchEntity>();
            ProcessStateRfidItem processStateRfidItem = null;
            ProcessStateChannelItem processStateChannelItem = null;

            foreach (var machineIdChannel in dicMachineIdChannel)
            {
                var temp = GetInpSearchDataS(param, machineIdChannel.Key);
                if (temp == null || temp.Count == 0)
                    continue;

                lstTempSearchInpS.AddRange(temp);
            }

            if (processStateRfidItem == null)
                processStateRfidItem = new ProcessStateRfidItem();

            if (lstTempSearchInpS.Count == 0)
                return new ProcessEntitiy();

            processStateRfidItem.Rfid = lstTempSearchInpS.Last().Rfid;

            foreach (var channel in lstTempSearchInpS.Select(x => x.ChName).Distinct())
            {
                lstTempSearchInpS = lstTempSearchInpS.ToList().FindAll(x => x.LowerLimit != "null" || x.UpperLimit != "null");
                var dicTempSearchInpS = lstTempSearchInpS.OrderBy(x => x.CollectDt)
                                                         .GroupBy(x => x.ChName, x => x)
                                                         .ToDictionary(x => x.Key, x => x.Select(x => DataInpEntity.EntityToItem(x)).ToList());

                if (processStateRfidItem.ChannelItemDic == null)
                    processStateRfidItem.ChannelItemDic = new Dictionary<string, ProcessStateChannelItem>();

                if (!dicTempSearchInpS.ContainsKey(channel))
                    continue;

                processStateChannelItem = new ProcessStateChannelItem();
                processStateChannelItem.LowerLimit = lstTempSearchInpS.Where(x => x.ChName == channel).Last().LowerLimit;
                processStateChannelItem.UpperLimit = lstTempSearchInpS.Where(x => x.ChName == channel).Last().UpperLimit;
                var calcData = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN").Select(x => Convert.ToDouble(x.Inp)).ToList();
                var dicCalcData = Utility.AddStatisticalRow(calcData);
                processStateChannelItem.Average = dicCalcData["avg"].ToString();
                processStateChannelItem.StandardDeviation = dicCalcData["standartdeviation"].ToString();
                processStateChannelItem.VarianceValue = dicCalcData["variance"].ToString();
                int index = 0;
                dicTempSearchInpS[channel].ForEach(x => x.Index = ++index);
                processStateChannelItem.TotalData = dicTempSearchInpS[channel];
                processStateChannelItem.LowerErrorDataKeyS = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN" && Convert.ToDouble(x.Inp) <= Convert.ToDouble(processStateChannelItem.LowerLimit))
                    .OrderBy(x => Convert.ToDouble(x.Inp))
                    .Select(x => x.Index)
                    .ToList();
                processStateChannelItem.UpperErrorDataKeyS = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN" && Convert.ToDouble(x.Inp) >= Convert.ToDouble(processStateChannelItem.UpperLimit))
                    .OrderBy(x => Convert.ToDouble(x.Inp))
                    .Select(x => x.Index)
                    .ToList();
                processStateChannelItem.LowerTop5DataKeyS = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN").OrderBy(x => Convert.ToDouble(x.Inp)).Take(5)
                    .OrderBy(x => Convert.ToDouble(x.Inp))
                    .Select(x => x.Index)
                    .ToList();
                processStateChannelItem.UpperTop5DataKeyS = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN").OrderByDescending(x => Convert.ToDouble(x.Inp)).Take(5)
                    .OrderByDescending(x => Convert.ToDouble(x.Inp))
                    .Select(x => x.Index)
                    .ToList();


                //var templowTop5 = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN").OrderBy(x => Convert.ToDouble(x.Inp)).Take(5)
                //    .OrderBy(x => Convert.ToDouble(x.Inp))
                //    .Select(x => new { x.Index, x.Inp })
                //    .ToList();

                //var tempupperTop5 = dicTempSearchInpS[channel].Where(x => x.Inp != "NaN").OrderByDescending(x => Convert.ToDouble(x.Inp)).Take(5)
                //    .OrderByDescending(x => Convert.ToDouble(x.Inp))
                //    .Select(x => new { x.Index, x.Inp })
                //    .ToList();

                processStateRfidItem.ChannelItemDic.Add(channel, processStateChannelItem);
            }
            processStateRfidItem.ChannelItemDic.Select(x => x.Key).OrderBy(x => x.Length).ThenBy(x => x);

            Dictionary<string, ProcessStateChannelItem> processStateItem = processStateRfidItem.ChannelItemDic.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
            processStateRfidItem.ChannelItemDic = processStateItem;

            Result.RfidItemList.Add(processStateRfidItem);

            return Result;
        }




        public static List<DataInpSearchEntity> GetInpSearchDataS(HybridDictionary param, string machineId)
        {
            Stopwatch stopwatch = new Stopwatch();
          
            List<DataInpSearchEntity> lstResultS = new List<DataInpSearchEntity>();

            // ShiftType (=workType)을 기준으로 시작날짜의 시간과 종료날짜의 시간 선언
            string sShiftType = param["workType"].ToString();
            DateTime dtStart = Convert.ToDateTime(param["startDt"].ToString());
            DateTime dtEnd = Convert.ToDateTime(param["endDt"].ToString());
            bool bIsContainMidNight = Utility.GetSchedule(sShiftType, ref dtStart, ref dtEnd);
            // -----------------------------------------------------------------------

            HybridDictionary queryParam = new HybridDictionary()
                {
                    { "machineId", machineId},
                    { "rfid", Convert.ToInt32(param["rfid"]).ToString() },
                    { "startDt", ""},
                    { "endDt", ""},
                };

            if (bIsContainMidNight)
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.Date.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                stopwatch.Start();
                lstResultS.AddRange(DataContext.StringEntityList<DataInpSearchEntity>("@Mold.SelectInpSearch", queryParam));
                stopwatch.Stop();
                Debug.Write( $"측정1 time:{stopwatch.ElapsedMilliseconds}ms\n");


                queryParam["startDt"] = dtStart.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                stopwatch.Start();
                lstResultS.AddRange(DataContext.StringEntityList<DataInpSearchEntity>("@Mold.SelectInpSearch", queryParam));
                stopwatch.Stop();
                Debug.Write($"측정2 time:{stopwatch.ElapsedMilliseconds}ms\n");
            }
            else
            {
                queryParam["startDt"] = dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff");
                queryParam["endDt"] = dtEnd.AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
                stopwatch.Start();
                lstResultS.AddRange(DataContext.StringEntityList<DataInpSearchEntity>("@Mold.SelectInpSearch", queryParam));
                stopwatch.Stop();
                Debug.Write($"측정2 time:{stopwatch.ElapsedMilliseconds}ms\n");
            }

            return lstResultS;
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            ProcessList molds = ListAllCache(corpCode);
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
