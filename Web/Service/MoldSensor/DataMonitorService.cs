using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Web
{
    public class DataMonitorService : BaseService, Map.IMap
    {
        #region Cache

        public static MonitorList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
                //Json 형식으로 값 전달
                // { "corpCode", corpCode }
            };

            return new MonitorList(DataContext.StringEntityList<DataMonitorEntity>("@Mold.MoldList", param));
        }

        public static MonitorList ListAllCache(string corpCode)
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

        public static MonitorList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static MonitorSelectList Select()
        {
            List<DataMonitorSelectEntity> list = DataContext.StringEntityList<DataMonitorSelectEntity>("@Mold.SelectMachine", new HybridDictionary());
            return new MonitorSelectList(list);
        }

        public static MoldList SelectMold()
        {
            List<MoldEntity> list = DataContext.StringEntityList<MoldEntity>("@Mold.SelectMold", new HybridDictionary());
            return new MoldList(list);
        }

        public static List<ChannelEntity> SelectChList(string machineId)
        {
            List<ChannelEntity> list = DataContext.StringEntityList<ChannelEntity>("@Mold.SelectChList", new HybridDictionary() { { "machineId", machineId } });
            return list;
        }

        public static MonitorList GetPressure(string cycleNo, string machineId, string machineType)
        {
            List<DataMonitorEntity> list = null;
            if (!string.IsNullOrEmpty(cycleNo))
            {
                HybridDictionary param = new HybridDictionary()
                {
                    {"cycleNo",cycleNo},
                    {"machineId" ,machineId},
                    {"machineType" ,machineType},
                };
                list = DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectOnePressure", param);
            }
            else
            {
                HybridDictionary param = new HybridDictionary()
                {
                    {"machineId" ,machineId},
                    {"machineType" ,machineType},
                };
                list = DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectPressure", param);
            }

            return new MonitorList(list);
        }

        public static MonitorList GetTemperture(string cycle, string machineId, string machineType)
        {
            List<DataMonitorEntity> list = null;
            if (!string.IsNullOrEmpty(cycle))
            {
                HybridDictionary param = new HybridDictionary()
                {
                    {"cycleNo",cycle},
                    {"machineId" ,machineId},
                    {"machineType" ,machineType},
                };
                list = DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectOneTemperature", param);
            }
            else
            {
                HybridDictionary param = new HybridDictionary()
                {
                    {"machineId" ,machineId},
                    {"machineType" ,machineType},
                };

                list = DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectTemperature", param);
            }
       
            return new MonitorList(list);
        }


        public static MonitorInplList GetIntegralPressure(HybridDictionary param)
        {
            List<DataInpCustomEntitiy> lstInpCustomS = new List<DataInpCustomEntitiy>();
            string[] arrInpKeys = new string[param.Keys.Count];
            param.Keys.CopyTo(arrInpKeys, 0);

            List<string> lstMachineIdS = JArray.Parse(param["machineId"].ToString()).ToObject<List<string>>();

            if (lstMachineIdS.Count == 0 || lstMachineIdS.First() == null || lstMachineIdS.First() == "")
            {
                //값이 없을경우 공백 retrun
                DataInpCustomEntitiy lnpData = new DataInpCustomEntitiy();
                lstInpCustomS.Add(lnpData);
                return new MonitorInplList(lstInpCustomS);
            }

            param["machineId"] = lstMachineIdS.First().ToString();
            List<DataInpEntity> list = DataContext.StringEntityList<DataInpEntity>("@Mold.SelectInp", param);

            DateTime startDt = DateTime.Now.Date;
            DateTime endDt = DateTime.Now.Date;
            Utility.GetSchedule("all", ref startDt, ref endDt);

            //조회된 값이 있을경우 
            if (list.Count > 0)
            {
                List<DataInpItem> lstValueS = new List<DataInpItem>();
                foreach (DataInpEntity oriInp in list)
                {
                    DataInpItem itemInp = new DataInpItem()
                    {
                        CycleNo = oriInp.CycleNo,
                        Inp = oriInp.Value,
                        CollectDt = oriInp.CollectDt,
                        MachineId = oriInp.MachineId,
                    };

                    lstValueS.Add(itemInp);
                }

                lstValueS = lstValueS.FindAll(x => Convert.ToDateTime(x.CollectDt) >= startDt && Convert.ToDateTime(x.CollectDt) < endDt);

                DataInpCustomEntitiy lnpData = new DataInpCustomEntitiy()
                {
                    MachineId = lstMachineIdS,
                    MachineType = param["machineType"].ToString(),
                    LowerLimit = list.First().LowerLimit,
                    UpperLimit = list.First().UpperLimit,
                    Values = lstValueS,
                };


                lstInpCustomS.Add(lnpData);
            }

            return new MonitorInplList(lstInpCustomS);
        }

        public static int Update(HybridDictionary param)
        {
            int bOK = -1;
            object colorS = param["colorS"];
            if (colorS == null) return bOK;
            var deserializeJson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, ColorState>>>(colorS.ToString());
            if (deserializeJson == null) return bOK;


            foreach (var dicKey in deserializeJson)
            {
                var chname = dicKey.First().Key;

                HybridDictionary customParam = new HybridDictionary()
                    {
                        {"machineId" ,param["machineId"]},
                        {"machineType" ,param["machineType"]},
                        {"chName" ,chname},
                        {"color" ,dicKey[chname].Color},
                        {"isVisible" ,dicKey[chname].Visible },
                    };
                bOK = DataContext.StringNonQuery("@Mold.MoldColorUpdate", customParam);
            }
            return bOK;
        }

        public static DetailCustomEntitiy GetSelectDetail(string machineId, string machineType,string cycleNo)
        {
            HybridDictionary param = new HybridDictionary()
            {
                { "cycleNo",cycleNo},
                {"machineId" ,machineId},
                {"machineType" ,machineType}
            };

            List < DataMonitorEntity > lstActivePs= DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectOnePressure", param);
            List < DataMonitorEntity > lstActiveTp = DataContext.StringEntityList<DataMonitorEntity>("@Mold.SelectOneTemperature", param);
            List<DataMonitorEntity> lstActiveChS = new List<DataMonitorEntity>();
            lstActiveChS.AddRange(lstActivePs);
            lstActiveChS.AddRange(lstActiveTp);
            //현재 수집 및 활성화된 CH 리스트가져오기 
            List<string> lstActiveChNameS =  lstActiveChS.Select(x => x.ChName).OrderBy(x => x.Length).ThenBy(x => x).ToList();


            List<DetailEntity> lstPressure = DataContext.StringEntityList<DetailEntity>("@Mold.SelectOnePressureDetail", param);
            List<DetailEntity> lstTemperature = DataContext.StringEntityList<DetailEntity>("@Mold.SelectOneTemperatureDetail", param);
            List<ActiveColorEntity> lstActiveColorS = DataContext.StringEntityList<ActiveColorEntity>("@Mold.SelectActiveColor", param);

            List<DetailEntity> lstTempS = new List<DetailEntity>();
            lstTempS.AddRange(lstPressure);
            lstTempS.AddRange(lstTemperature);

            List<DetailItem> lstItemS = new List<DetailItem>();
            DetailCustomEntitiy dtCustom = new DetailCustomEntitiy();


            for (int i = 0; i < lstTempS.Count; i++)
            {
                DetailItem tempItem;
                if (!lstItemS.Any(x => x.ChName == lstTempS[i].ChName))
                {
                    tempItem = new DetailItem();
                    tempItem.MachineId = lstTempS[i].MachineId;
                    tempItem.ChName = lstTempS[i].ChName;
                    tempItem.Color = lstTempS[i].Color;
                    lstItemS.Add(tempItem);
                }

                tempItem = lstItemS.Find(x => x.ChName == lstTempS[i].ChName) as DetailItem;

                if (tempItem!= null)
                {
                    tempItem.ChName = lstTempS[i].ChName;
                    tempItem.Color = lstTempS[i].Color;

                    switch (lstTempS[i].DataType)
                    {
                        case "Maximum_Temperature":
                            tempItem.MaxTemperature = lstTempS[i].Value;
                            break;
                        case "Maximum_Pressure":
                            tempItem.MaxPressure = lstTempS[i].Value;
                            break;
                        case "MoldTemp_Temperature":
                            tempItem.MoldTemp = lstTempS[i].Value;
                            break;
                        case "Meltfront_Temperature":
                            tempItem.MeltFront = lstTempS[i].Value;
                            break;
                        case "Integral_Temperature":
                            tempItem.IntergralTemperature = lstTempS[i].Value;
                            break;
                        case "Integral_Pressure":
                            tempItem.IntergralPressure = lstTempS[i].Value;
                            break;
                    }
                }

                dtCustom.CycleNo = lstTempS[i].CycleNo;
            }


            if (lstItemS.Count != lstActiveChNameS.Count)
            {
                for (int index = 0; index < lstActiveChNameS.Count; index++)
                {
                    var value = lstItemS.Find(x => x.ChName == lstActiveChNameS[index]);
                    var color = lstActiveColorS.Find(x => x.ChName == lstActiveChNameS[index]);
                    if (value == null)
                    {
                        lstItemS.Add(new DetailItem() { ChName = lstActiveChNameS[index], Color = color.Color });
                    }
                }
            }

            dtCustom.Values = lstItemS.OrderBy(x => x.ChName.Length).ThenBy(x => x.ChName).ToList();

            return dtCustom;
        }

        public static List<AlarmEntitiy> GetSelectAlarm()
        {
            List<AlarmEntitiy> lstAlarmS = DataContext.StringEntityList<AlarmEntitiy>("@Mold.SelectAlarm", new HybridDictionary());
            DateTime startDt = DateTime.Now.Date;
            DateTime endDt = DateTime.Now.Date;
            Utility.GetSchedule("all", ref startDt, ref endDt);
            lstAlarmS = lstAlarmS.FindAll(x=> Convert.ToDateTime(x.CollectDt) >= startDt && Convert.ToDateTime(x.CollectDt) < endDt);
            return lstAlarmS;
        }


        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            MonitorList molds = ListAllCache(corpCode);
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

