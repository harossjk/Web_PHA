using Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web
{
    public static class Utility
    {

        #region Utility

        // 가로로 들어온 데이터를 새로로 변경하기위해 사용하는 데이터 정리 함수
        public static Dictionary<string, Dictionary<string, string[]>> DataCleanup(List<ChannelEntity> lstChS,List<DataMonitorEntity> lstOriDataS)
        {
            Dictionary<string, Dictionary<string, string[]>> dicValueS = new Dictionary<string, Dictionary<string, string[]>>();
            Dictionary<string, string[]> dicValue = new Dictionary<string, string[]>();
            var customData = lstOriDataS.GroupBy(x => x.ChName);
            string sCycleNo = "";
            foreach (var key in customData)
            {
                foreach (var data in key)
                {
                    if (!string.IsNullOrEmpty(data.Value))
                    {
                 
                        sCycleNo = data.CycleNo;
                        string sChName = data.ChName;
                        string[] pstrSplitData = data.Value.Split('|');

                        //Data 홀수 압축
                        List<string> tempSplitData = new List<string>();
                        for (int i = 0; i < pstrSplitData.Length; i++)
                            if (i % 3 == 0)
                                tempSplitData.Add(pstrSplitData[i]);

                        pstrSplitData = tempSplitData.ToArray();

                        if (!dicValue.ContainsKey(sChName))
                        {
                            if (!data.IsVisible)
                                continue;

                            dicValue.Add(sChName, pstrSplitData);
                        }
                    }
                }
            }

            dicValue = dicValue.OrderBy(x => x.Key.Length).ThenBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
            dicValueS.Add(sCycleNo, dicValue);

            return dicValueS;
        }

        // React 그래프에 필요한 데이터 형식을 재정의 하는 함수
        public static List<DataMonitorEntity> DataReassembly(Dictionary<string, Dictionary<string, string[]>> dicCurrData)
        {
            List<DataMonitorEntity> lstTempData = new List<DataMonitorEntity>();
            List<CMinMaxValue> pMinMaxValue = new List<CMinMaxValue>();
            List<CMinMaxValue> tMinMaxValue = new List<CMinMaxValue>();

            foreach (string key in dicCurrData.Keys)
            {
                Dictionary<string, string[]> valueS = dicCurrData[key];
                List<dynamic> lstDynamic = new List<dynamic>();
                dynamic dyData = null;

                List<int> lstSizeS = new List<int>();
                foreach (string valueKey in valueS.Keys)
                    lstSizeS.Add(valueS[valueKey].Length);
                int maxSize = lstSizeS.OrderByDescending(i => i).FirstOrDefault();

                DataMonitorEntity dt = new DataMonitorEntity();
                dt.CycleNo = key;
                dt.Data = lstDynamic;

                foreach (string chkey in valueS.Keys)
                {
                    Dictionary<string, string> dicData = new Dictionary<string, string>();

                    dt.ChName += chkey + ",";
                    string[] arrData = valueS[chkey];

                    if (arrData.Length != 0)
                    {
                        List<double> ints = arrData.ToList()
                        .Select(s => double.TryParse(s, out double n) ? n : (double?)null)
                        .Where(n => n.HasValue)
                        .Select(n => n.Value)
                        .ToList();

                        double dMin = ints.Min();
                        double dMax = ints.Max();

                        if (chkey.Contains("P"))
                            pMinMaxValue.Add(new CMinMaxValue() { ChName = chkey, Min = dMin, Max = dMax });
                        else if (chkey.Contains("T"))
                            tMinMaxValue.Add(new CMinMaxValue() { ChName = chkey, Min = dMin, Max = dMax });
                    }

                    if (lstDynamic.Count != 0)
                    {
                        if (lstDynamic.Count >= arrData.Length)
                        {
                            for (int index0 = 0; index0 < maxSize; index0++)
                            {
                                Dictionary<string, string> obj = lstDynamic[index0] as Dictionary<string, string>;
                                if (arrData.Length <= index0)
                                    break;
                                dicData.Add(chkey, arrData[index0]);
                                dyData = dicData;
                                obj.Add(chkey, arrData[index0]);
                                dicData = new Dictionary<string, string>();
                            }
                        }
                    }
                    else
                    {
                        for (int index1 = 0; index1 < arrData.Length; index1++)
                        {
                            dicData.Add(chkey, arrData[index1]);
                            dyData = dicData;
                            lstDynamic.Add(dyData);
                            dicData = new Dictionary<string, string>();
                        }
                    }
                }

                if(pMinMaxValue.Count!=0 && tMinMaxValue.Count != 0)
                {
                    dt.PLowerLimit = pMinMaxValue.Min(x => x.Min).ToString();
                    dt.PUpperLimit = pMinMaxValue.Max(x => x.Max).ToString();
                    dt.TLowerLimit = tMinMaxValue.Min(x => x.Min).ToString();
                    dt.TUpperLimit = tMinMaxValue.Max(x => x.Max).ToString();
                }
                
                
                lstTempData.Add(dt);
            }


            return lstTempData;
        }

        // React에서 필요한 색상에 대해 각채널 색상에 대하여 재정의 하는 로직
        public static List<DataMonitorEntity> ColorReassembly(List<DataMonitorEntity> lstRedeData, List<DataMonitorEntity> lstOriDataS)
        {
            for (int index0 = 0; index0 < lstRedeData.Count; index0++)
            {
                var  strSplitData =  lstOriDataS.Select(x => x.ChName).ToArray();
                List<Dictionary<string, ColorState>> lstDynamic = new List<Dictionary<string, ColorState>>();
                for (int index1 = 0; index1 < strSplitData.Length; index1++)
                {
                    Dictionary<string, ColorState> dicData = new Dictionary<string, ColorState>();
                    DataMonitorEntity dt = lstOriDataS.Find(x => x.CycleNo == lstRedeData[index0].CycleNo && x.ChName == strSplitData[index1]);
                    if (dt != null)
                    {
                        //lstRedeData[index0].ChName = strSplitData[index1];
                        lstRedeData[index0].MachineName = dt.MachineName;
                        lstRedeData[index0].MachineType = dt.MachineType;
                        lstRedeData[index0].MachineId = dt.MachineId;
                        lstRedeData[index0].MachineLegend = dt.MachineLegend;
                        lstRedeData[index0].ViewStatus = dt.ViewStatus;
                        lstRedeData[index0].SensorIsActive = dt.SensorIsActive;
                        ColorState colorState = new ColorState() { Color=dt.Color,Visible= dt.IsVisible};
                        dicData.Add(strSplitData[index1],colorState);
                        lstDynamic.Add(dicData);
                    }
                    else
                    {
                        dt = lstOriDataS.Find(x => x.CycleNo == lstRedeData[index0].CycleNo && x.ChName == strSplitData[index1]);
                        if (dt != null)
                        {
                            ColorState colorState = new ColorState() { Color = dt.Color, Visible = dt.IsVisible };
                            dicData.Add(strSplitData[index1], colorState);
                            lstDynamic.Add(dicData);
                        }
                    }
                }

                lstRedeData[index0].ColorS = lstDynamic;
            }

            return lstRedeData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sShiftType"></param>
        /// <param name="_dtStartDate"></param>
        /// <param name="_dtEndDate"></param>
        /// <returns></returns>
        public static bool GetSchedule(string _sShiftType, ref DateTime _dtStartDate, ref DateTime _dtEndDate)
        {
            bool bIsContainMidNight = true;

            switch (_sShiftType.ToLower())
            {
                case "all":
                    _dtStartDate = _dtStartDate.Date.AddHours(8);
                    _dtEndDate = _dtEndDate.Date.AddDays(1).AddHours(8);
                    break;

                case "day":
                    _dtStartDate = _dtStartDate.Date.AddHours(8);
                    _dtEndDate = _dtEndDate.Date.AddHours(20);
                    bIsContainMidNight = false;
                    break;

                case "night":
                    _dtStartDate = _dtStartDate.Date.AddHours(20);
                    _dtEndDate = _dtEndDate.Date.AddDays(1).AddHours(8);
                    break;

                default:
                    _dtStartDate = _dtStartDate.Date;
                    _dtEndDate = _dtEndDate.Date.AddDays(1);
                    break;
            }

            return bIsContainMidNight;
        }

        public static DateTime GetDate(DateTime CollectDt)
        {
            DateTime dtResult, dtStartTime, dtEndTime;
            dtResult = dtStartTime = dtEndTime = CollectDt.Date;

            GetSchedule("all", ref dtStartTime, ref dtEndTime);

            if (CollectDt.Hour < dtStartTime.Hour)
                dtResult.AddDays(-1);

            return dtResult;
        }

        public static Dictionary<string, double> AddStatisticalRow(List<double> lstData)
        {
            //소숫점 자릿수
            int m_iDigits = 3;
            //합계
            double dSum = 0;
            //평균
            double dAvg = 0;
            //최소
            double dMin = 0;
            //최대
            double dMax = 0;
            //표준 편차
            double dStandardDiviation = 0;
            //분산
            double dVariance = 0;

            foreach (double val in lstData)
            {
                if (dMin == 0)
                {
                    dMin =Convert.ToDouble(val);
                }
                else
                {
                    if (dMin > Convert.ToDouble(val))
                        dMin = Convert.ToDouble(val);
                }

                if (dMax == 0)
                {
                    dMax = Convert.ToDouble(val);
                }
                else
                {
                    if (dMax < Convert.ToDouble(val))
                        dMax = Convert.ToDouble(val);
                }

                dSum += Convert.ToDouble(val);
            }

            dAvg = dSum / lstData.Count;
            dVariance = CalcVariance(lstData, dAvg);
            dStandardDiviation = Math.Sqrt(dVariance);

            Dictionary<string, double> dicVarianceS = new Dictionary<string, double>();
            dicVarianceS.Add("lowerLimit",Math.Round(dMin, m_iDigits));
            dicVarianceS.Add("upperLimit",Math.Round(dMax, m_iDigits));
            dicVarianceS.Add("avg",Math.Round(dAvg, m_iDigits));
            dicVarianceS.Add("variance",Math.Round(dVariance, m_iDigits));
            dicVarianceS.Add("standartdeviation", Math.Round(dStandardDiviation, m_iDigits));
            return dicVarianceS;
        }

        //분산 계산
        private static double CalcVariance(List<double> lstData, double dAvg)
        {
            //소숫점 자릿수
            int m_iDigits = 3;
            //제곱의 합
            double dSum = 0;
            foreach (double val in lstData)
            {
                //2 제곱
                double dSquare = Math.Pow((Convert.ToDouble(val) - dAvg), 2);

                dSum += dSquare;
            }

            return Math.Round((dSum / lstData.Count), m_iDigits);
        }

        public static DataTable ConvertToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        #endregion
    }
}
