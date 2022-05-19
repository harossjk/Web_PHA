using Framework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace Web
{
    public class ExportService
    {
        //서버 개시시 m_isLocalMode 를 false 로 하면 서버 주소에 ExcelTemplate 파일이 만들어지며 해당 폴더에 
        //각 페이지 Export excel 파일을 복사 붙여넣기 진행
        private static bool m_isLocalMode = true;
        private static string m_sSever = $"C:\\Publish\\PHC_MoldMonitoringSystem\\ExcelTemplate";
        private static string m_sLocalHost = System.IO.Directory.GetCurrentDirectory() + @"\ExcelTemplate";

        public static void CreateDirectory()
        {
            try
            {
                System.IO.DirectoryInfo directory = null;
                if (m_isLocalMode)
                    directory = new System.IO.DirectoryInfo(m_sLocalHost);
                else
                    directory = new System.IO.DirectoryInfo(m_sSever);

                if (!directory.Exists) { directory.Create(); }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        public static MemoryStream ExportProcess(HybridDictionary dic,ProcessEntitiy searchData, string sFileName)
        {
            string sTemplatePath = string.Empty;
            if (m_isLocalMode)
                sTemplatePath = m_sLocalHost + "\\" + sFileName;
            else
                sTemplatePath = m_sSever + "\\" + sFileName;

            DateTime startDt = DateTime.Parse(dic["startDt"].ToString());
            DateTime endtDt = DateTime.Parse(dic["endDt"].ToString());
            string workType = dic["workType"].ToString();
            Utility.GetSchedule(workType, ref startDt, ref endtDt);
            workType= workType == "day" ? "주간" : dic["workType"].ToString() == "night" ? "야간" : "종일";

            var file = new FileInfo(sTemplatePath);
            using (var package = new ExcelPackage(file))
            {
                #region 표지

                //리포트 시작, 리포트 종료
                var sheet1 = package.Workbook.Worksheets[0];
                sheet1.Cells["X19"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet1.Cells["X22"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion

                var sheet2 = package.Workbook.Worksheets[1];
                sheet2.Cells["K3"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet2.Cells["K4"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");

                //압력적분 데이터
                string[] inpDataPos = { "C", "D", "E", "F", "G", "H", "I", "J", "K" };

                //Top 5 데이터
                string[] errTopPos = { "N", "Q", "T", "W", "Z" };

                //설비, RFID , 금형명, 근무타입, CycleNo, 압력적분, 상한값, 하한값 , 수집시간 
                List<List<object>> inpDataS = new List<List<object>>();
                ExcelWorksheet sheet = null;
                foreach (var item in searchData.RfidItemList)
                {
                    int rfid = int.Parse(item.Rfid);

                    //채널별로 Sheet 만들기 
                    foreach (var diCh in item.ChannelItemDic)
                    {
                        ProcessStateChannelItem getValue = item.ChannelItemDic[diCh.Key];
                        if (getValue != null && getValue.LowerTop5DataKeyS.Count <= 0 && getValue.UpperTop5DataKeyS.Count <= 0)
                            continue;

                        List<ProcessStateItem> lstNanS = getValue.TotalData.FindAll(x => x.Inp != "NaN" && x.Inp!="0").ToList();
                        if(lstNanS!=null&& lstNanS.Count==0)
                            continue;


                        sheet = package.Workbook.Worksheets.Add(diCh.Key, sheet2);

                        #region 공정능력 지수 통계

                        int upperValue = getValue.UpperLimit == "(null)" ? 0 : int.Parse(getValue.UpperLimit);
                        int lowerValue = getValue.LowerLimit == "(null)" ? 0 : int.Parse(getValue.LowerLimit);

                        foreach (ProcessStateItem dataItem in getValue.TotalData)
                        {
                            List<object> inpItem = new List<object>();
                            string machineId = dataItem.MachineId + "호기";
                            string moldName = dic["moldName"].ToString();
                            string cycleNo = dataItem.CycleNo;
                            double inpValue = double.Parse(dataItem.Inp);
                            string collectDt = dataItem.CollectDt.ToString("yyyy-MM-dd HH:mm:ss");

                            inpItem.Add(machineId);
                            inpItem.Add(rfid);
                            inpItem.Add(moldName);
                            inpItem.Add(workType);
                            inpItem.Add(cycleNo);
                            inpItem.Add(inpValue);
                            inpItem.Add(upperValue);
                            inpItem.Add(lowerValue);
                            inpItem.Add(collectDt);

                            inpDataS.Add(inpItem);
                        }

                        ExcelEx.InputWorkSheetCellData(sheet, inpDataPos, 29, inpDataS);

                        //Series 차트편집
                        var chart = sheet.Drawings.First() as OfficeOpenXml.Drawing.Chart.ExcelLineChart;
                        int iLastAddress = inpDataS.Count;
                        var label = sheet.Cells["H28:J28"].ToString();
                        chart.Series[0].XSeries = label;
                        chart.Series[1].XSeries = label;
                        chart.Series[2].XSeries = label;
                        string sRangeH = $"H{29}:H{iLastAddress}";
                        string sRangeI = $"I{29}:I{iLastAddress}";
                        string sRangeJ = $"J{29}:J{iLastAddress}";
                        chart.Series[0].Series = sheet.Cells[sRangeH].FullAddress;
                        chart.Series[1].Series = sheet.Cells[sRangeI].FullAddress;
                        chart.Series[2].Series = sheet.Cells[sRangeJ].FullAddress;
                        inpDataS.Clear();

                        #endregion

                        #region Error Top 5

                        #region Upper

                        int nextUpperIndex = 0;
                        var errUpper = getValue.UpperTop5DataKeyS;
                        foreach (var errKey in errUpper)
                        {
                            var errValue = getValue.TotalData.Find(x => x.Index == errKey);
                            if (errValue == null)
                                continue;

                            //상한 오차율 계산
                            double errUpperRate = (Math.Abs(Convert.ToDouble(upperValue) - Convert.ToDouble(errValue.Inp)) / Convert.ToDouble(upperValue)) * 100;
                            errUpperRate = Math.Round(errUpperRate, 2);
                            
                            sheet.Cells[errTopPos[nextUpperIndex] + "9"].Value = errValue.MachineId;//사출기
                            sheet.Cells[errTopPos[nextUpperIndex] + "10"].Value = errValue.CycleNo;//사이클 번호
                            sheet.Cells[errTopPos[nextUpperIndex] + "11"].Value = errUpperRate;//오차율
                            sheet.Cells[errTopPos[nextUpperIndex] + "12"].Value = errValue.Inp;//압력적분값
                            sheet.Cells[errTopPos[nextUpperIndex] + "13"].Value = upperValue;//상한압력적분값
                            sheet.Cells[errTopPos[nextUpperIndex] + "14"].Value = errValue.CollectDt.Date.ToString("yyyy-MM-dd");//수집일
                            sheet.Cells[errTopPos[nextUpperIndex] + "15"].Value = errValue.CollectDt.TimeOfDay.ToString();//수집시간
                            nextUpperIndex++;
                        }

                        #endregion

                        #region Lower
                        nextUpperIndex = 0;
                        var errLower = getValue.LowerTop5DataKeyS;
                        foreach (var errKey in errLower)
                        {
                            var errValue = getValue.TotalData.Find(x => x.Index == errKey);
                            if (errValue == null)
                                continue;

                            //하한 오차율 계산
                            double errLowerRate = (Math.Abs(Convert.ToDouble(lowerValue) - Convert.ToDouble(errValue.Inp)) / Convert.ToDouble(lowerValue)) * 100;
                            errLowerRate = Math.Round(errLowerRate, 2);

                            sheet.Cells[errTopPos[nextUpperIndex] + "20"].Value = errValue.MachineId;//사출기
                            sheet.Cells[errTopPos[nextUpperIndex] + "21"].Value = errValue.CycleNo;//사이클 번호
                            sheet.Cells[errTopPos[nextUpperIndex] + "22"].Value = errLowerRate;//오차율
                            sheet.Cells[errTopPos[nextUpperIndex] + "23"].Value = errValue.Inp;//압력적분값
                            sheet.Cells[errTopPos[nextUpperIndex] + "24"].Value = lowerValue;//하한압력적분값
                            sheet.Cells[errTopPos[nextUpperIndex] + "25"].Value = errValue.CollectDt.Date.ToString("yyyy-MM-dd");//수집일
                            sheet.Cells[errTopPos[nextUpperIndex] + "26"].Value = errValue.CollectDt.TimeOfDay.ToString();//수집시간
                            nextUpperIndex++;
                        }

                        #endregion

                        #endregion

                        #region 분포표
                        int totalCount = getValue.TotalData.Count;// 전체 개수
                        int lowerCount = getValue.LowerErrorDataKeyS.Count;
                        int upperCount = getValue.UpperErrorDataKeyS.Count;
                        int normalCount = totalCount - (lowerCount + upperCount);

                        sheet.Cells["M31"].Value = lowerValue; //하한값
                        sheet.Cells["M33"].Value = getValue.Average;//평균
                        sheet.Cells["M35"].Value = upperValue;//상한값
                        sheet.Cells["M37"].Value = getValue.VarianceValue;//분산값
                        sheet.Cells["S31"].Value = getValue.LowerErrorDataKeyS.Count;//하한 에러 수량
                        sheet.Cells["V31"].Value = normalCount;//분포표 안에 있는 데이터 수량
                        sheet.Cells["Y31"].Value = getValue.UpperErrorDataKeyS.Count;//상한 하한 수량

                        double calLowerLimitPercent =  (Convert.ToDouble(lowerCount) / Convert.ToDouble(totalCount)) * 100;
                        double calUpperLimitPercent = (Convert.ToDouble(upperCount) / Convert.ToDouble(totalCount)) * 100;
                        double calNormalPercent = (100 - calLowerLimitPercent) + calUpperLimitPercent;
                        
                        string sLowerPercent = string.Format("{0:N4}%", calLowerLimitPercent);
                        string sUpperPercent = string.Format("{0:N4}%", calUpperLimitPercent);
                        string sNormalPercent = string.Format("{0:N4}%", calNormalPercent);

                        sheet.Cells["S33"].Value = sLowerPercent;
                        sheet.Cells["V33"].Value = sNormalPercent;
                        sheet.Cells["Y33"].Value = sUpperPercent;

                
                        if (getValue.LowerErrorDataKeyS.Count>0)
                        {
                            int stNum = 35;
                            foreach (var lowerItem in getValue.LowerErrorDataKeyS)
                            {
                                var lowerErrItem = getValue.TotalData.Find(x=>x.Index==lowerItem);
                                if (lowerErrItem == null)
                                    continue;

                                sheet.Cells["S" + stNum].Value = lowerErrItem.CycleNo;
                                sheet.Cells["T" + stNum].Value = lowerErrItem.CollectDt.ToString("yyyy-MM-dd HH:mm:ss");
                                stNum++;
                            }
                        }

                        if (getValue.UpperErrorDataKeyS.Count > 0)
                        {
                            int stNum = 35;
                            foreach (var uppperItem in getValue.UpperErrorDataKeyS)
                            {
                                var upperErrItem = getValue.TotalData[uppperItem];
                                if (upperErrItem == null)
                                    continue;

                                sheet.Cells["Y" + stNum].Value = upperErrItem.CycleNo;
                                sheet.Cells["Z" + stNum].Value = upperErrItem.CollectDt.ToString("yyyy-MM-dd HH:mm:ss");
                                stNum++;
                            }
                        }

                        #endregion

                    }
                }

                package.Workbook.Worksheets["default"].Hidden = eWorkSheetHidden.VeryHidden;
                //첫번째 sheet 선택 
                package.Workbook.Worksheets.First().Select();

                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
                
        }

        public static MemoryStream ExportErrorState(HybridDictionary dic, List<ErrorEntity> searchData, string sFileName)
        {
         
            string sTemplatePath = string.Empty;
            if (m_isLocalMode)
                sTemplatePath = m_sLocalHost + "\\" + sFileName;
            else
                sTemplatePath = m_sSever + "\\" + sFileName;

            string workType = dic["workType"].ToString();
            DateTime startDt = DateTime.Parse(dic["startDt"].ToString());
            DateTime endtDt = DateTime.Parse(dic["endDt"].ToString());
            Utility.GetSchedule(workType, ref startDt, ref endtDt);

            var customData = searchData.GroupBy(x => x.Rfid).ToDictionary(key => key.Key, value => value.GroupBy(x => x.MachineId).ToDictionary(key => key.Key, value => value.Select(x => x)));

            var file = new FileInfo(sTemplatePath);
            using (var package = new ExcelPackage(file))
            {
                int cellStartNum = 26;

                #region 표지

                //리포트 시작, 리포트 종료
                var sheet1 = package.Workbook.Worksheets[0];
                sheet1.Cells["X19"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet1.Cells["X22"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion

                #region 종합 이상 통계

                //종합 이상 통계 시트 
                //리포트 시작, 리포트 종료
                var sheet2 = package.Workbook.Worksheets[1];
                sheet2.Cells["N3"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet2.Cells["N4"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");
                
                
                string[] errBasePos = { "C", "D", "E", "F", "G"};
                //이상건수 기준 통계 
                //설비,RFID, 금형명, 근무타입, 이상건수
                List<List<object>> errBaseStaticS = new List<List<object>>();
                var group = searchData.GroupBy(x => x.MachineId).ToDictionary(x => x.Key, y => y.ToList());

                foreach (var err in group)
                {
                    List<object> rowData = new List<object>();
                    string machineName = err.Key+"설비";
                    int rfid = int.Parse(err.Value[0].Rfid);
                    string moldName = err.Value[0].MoldName;
                    workType = workType == "day" ? "주간" : workType == "night" ? "야간" : "종일";
                    int totalCount = err.Value.Count;

                    rowData.Add(machineName);
                    rowData.Add(rfid);
                    rowData.Add(moldName);
                    rowData.Add(workType);
                    rowData.Add(totalCount);

                    errBaseStaticS.Add(rowData);
                }

                ExcelEx.InputWorkSheetCellData(sheet2, errBasePos, cellStartNum, errBaseStaticS);

                //종합 이상 통계 알람 내역
                //설비,RFID,금형명,메세지,발생시각,에러타입
                string[] errAlarmPos = { "I", "J", "K", "L", "M", "N" ,"O","P"};
                List < List<object>> errAlarmS = new List<List<object>>();
                for (int i = 0; i < searchData.Count; i++)
                {
                    List<object> rowData = new List<object>();
                    string machineName = searchData[i].MachineId + "호기";
                    int rfid = int.Parse(searchData[i].Rfid);
                    string moldName = searchData[i].MoldName;
                    string dataType = searchData[i].DataType;
                    string errorMsg = searchData[i].Message;
                    double value = searchData[i].Value== null? 0:  double.Parse(searchData[i].Value);
                    double limitValue = searchData[i].LimitValue== null ? 0 : double.Parse(searchData[i].LimitValue);
                    string errDt = searchData[i].CollectDt.ToString("yyyy-MM-dd HH:mm:ss");
                    

                    rowData.Add(machineName);
                    rowData.Add(rfid);
                    rowData.Add(moldName);
                    rowData.Add(dataType);
                    rowData.Add(errorMsg);
                    rowData.Add(value);
                    rowData.Add(limitValue);
                    rowData.Add(errDt);
                    errAlarmS.Add(rowData);
                }

                ExcelEx.InputWorkSheetCellData(sheet2, errAlarmPos, cellStartNum, errAlarmS);

                #endregion

                #region 상세 이상 통계 

                ExcelWorksheet sheet =null;
                foreach (var micID in group)
                {
                    if (package.Workbook.Worksheets.Any(x => x.Name == micID.Key + "호기"))
                        sheet = package.Workbook.Worksheets[micID.Key + "호기"];
                    else
                        sheet = package.Workbook.Worksheets.Add(micID.Key + "호기", package.Workbook.Worksheets[2]);

                    //title 변경
                    sheet.Cells["E3"].Value = sheet.Cells["E3"].Value.ToString().Replace("26호기", micID.Key.ToString()+"호기");

                    //차트변경 sheet 범위 재설정
                    var chart = sheet.Drawings.First() as OfficeOpenXml.Drawing.Chart.ExcelBarChart;
                    chart.Series[0].XSeries = sheet.Cells[26, 3, 33, 3].FullAddress;
                    chart.Series[0].Series = sheet.Cells[26, 6, 33, 6].FullAddress;
                    
                    var groupbyErrorType = micID.Value.GroupBy(x => x.ErrorType).ToDictionary(x => x.Key, x => x.Select(x => x).Count());
                    Dictionary<string, string> dicName = new Dictionary<string, string>();
                    dicName.Add("Integral_Pressure/UpperError", "F" + sheet.Cells["G26"].Start.Row.ToString());
                    dicName.Add("Integral_Pressure/LowerError", "F" + sheet.Cells["G27"].Start.Row.ToString());
                    dicName.Add("Maximum_Pressure/UpperError", "F" + sheet.Cells["G28"].Start.Row.ToString());
                    dicName.Add("Maximum_Pressure/LowerError", "F" + sheet.Cells["G29"].Start.Row.ToString());
                    dicName.Add("MoldTemp_Temperature/UpperError", "F" + sheet.Cells["G30"].Start.Row.ToString());
                    dicName.Add("MoldTemp_Temperature/LowerError", "F" + sheet.Cells["G31"].Start.Row.ToString());
                    dicName.Add("Maximum_Temperature/UpperError", "F" + sheet.Cells["G32"].Start.Row.ToString());
                    dicName.Add("Maximum_Temperature/LowerError", "F" + sheet.Cells["G33"].Start.Row.ToString());

                    foreach (var item in groupbyErrorType)
                        if (dicName.ContainsKey(item.Key))
                            sheet.Cells[dicName[item.Key]].Value = int.Parse(item.Value.ToString());

                    sheet.Cells["I3"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                    sheet.Cells["I4"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                #endregion

                //첫번째 sheet 선택 
                package.Workbook.Worksheets.First().Select();
                package.Workbook.Worksheets["default"].Hidden = eWorkSheetHidden.VeryHidden;
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        public static MemoryStream ExportRawData(HybridDictionary dic, List<RowItem> searchData,string sFileName)
        {
            string sTemplatePath = string.Empty;
            if (m_isLocalMode)
                sTemplatePath = m_sLocalHost + "\\" + sFileName;
            else
                sTemplatePath = m_sSever + "\\" + sFileName;

            string workType = dic["workType"].ToString();
            DateTime startDt = DateTime.Parse(dic["startDt"].ToString());
            DateTime endtDt = DateTime.Parse(dic["endDt"].ToString());
            Utility.GetSchedule(workType, ref startDt, ref endtDt);
            workType = workType == "day" ? "주간" : dic["workType"].ToString() == "night" ? "야간" : "종일";

            var file = new FileInfo(sTemplatePath);
            using (var package = new ExcelPackage(file))
            {
                #region 표지

                //리포트 시작, 리포트 종료
                var sheet1 = package.Workbook.Worksheets[0];
                sheet1.Cells["X19"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet1.Cells["X22"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion

                #region RawData 이력 리포트        
                List<List<object>> rawDataReportS = new List<List<object>>();
                string[] rowDataPos = { "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" ,"N"};

                var sheet2 = package.Workbook.Worksheets[1];
                sheet2.Cells["N3"].Value = startDt.ToString("yyyy-MM-dd HH:mm:ss");
                sheet2.Cells["N4"].Value = endtDt.ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < searchData.Count; i++)
                {
                    List<object> rowData = new List<object>();
                    rowData.Add(searchData[i].MachineType);
                    rowData.Add(searchData[i].MachineId); 
                    rowData.Add(searchData[i].MoldName); 
                    rowData.Add(searchData[i].Rfid);
                    rowData.Add(searchData[i].CollectDt);
                    rowData.Add(searchData[i].CycleNo);
                    rowData.Add(searchData[i].ChName);
                    rowData.Add(searchData[i].Maximum_Temperature);     //최대온도
                    rowData.Add(searchData[i].Maximum_Pressure);        //최대압력
                    rowData.Add(searchData[i].Integral_Pressure);       //압력적분
                    rowData.Add(searchData[i].Meltfront_Temperature);   //융융선단온도
                    rowData.Add(searchData[i].Mold_Temperature);        //금형온도
                    rawDataReportS.Add(rowData);
                }

                ExcelEx.InputWorkSheetCellData(sheet2, rowDataPos, 9, rawDataReportS);

                #endregion

                //첫번째 sheet 선택 
                package.Workbook.Worksheets.First().Select();
                //package.Workbook.Worksheets["default"].Hidden = eWorkSheetHidden.VeryHidden;

                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }
    }
}

