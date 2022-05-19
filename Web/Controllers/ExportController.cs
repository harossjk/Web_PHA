using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Transactions;
using System.IO;
using OfficeOpenXml;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBaseEx
    {

        


        

        private readonly ILogger<TempController> _logger;

        public ExportController(ILogger<TempController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 캐시사용
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public TempList Get()
        {
            TempList tempList = TempService.List(base.UserCorpCode);
            return tempList;
        }


        [HttpPost]
        [Route("download")]
        public IActionResult Download(HybridDictionary dic)
        {
            
            return File("", "", ""); ;
        }

        [HttpGet]
        [Route("download")]
        public IActionResult Download(string rfid,string moldName ,string workType,  string startDt, string endDt, string pageName)
        {
            if (rfid == null || workType == null || startDt == null || endDt == null || pageName == null|| moldName == null)
                return null;

            if (moldName.Contains("-"))
                moldName = moldName.Replace("-", "#");

            HybridDictionary dic = new HybridDictionary()
            {
                { "rfid", Convert.ToInt32(rfid).ToString() },
                { "moldName", moldName},
                { "workType", workType},
                { "startDt",  Convert.ToDateTime(startDt) },
                { "endDt", Convert.ToDateTime(endDt)},
                { "pageName", pageName},
            };

            MemoryStream memorStream = null;
            string fileName = string.Empty;
            ExportService.CreateDirectory();

            if (pageName.Equals("ProcessAbilityPage"))
            {
                ProcessEntitiy customData = ProcessStateService.GetIntegralPressureSearch(dic);
                if (customData.RfidItemList == null )
                    return Content("<html><head><script>alert('다운로드 데이터가 없습니다.'); history.back();</script></head></html>", "text/html; charset=utf-8");

                memorStream = ExportService.ExportProcess(dic, customData, "PROCESS_ABILITY_REPORT.xlsx");
                fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_[{int.Parse(rfid)} ({moldName})]공정능력 지수 통계.xlsx";
            }
            else if (pageName.Equals("ErrorStatisticPage"))
            {
                List<ErrorEntity> customData = ErrorStateService.GetErrorSearch(dic);
                if (customData == null || customData.Count==0)
                    return Content("<html><head><script>alert('다운로드 데이터가 없습니다.'); history.back();</script></head></html>", "text/html; charset=utf-8");

                //moldName = customData.Find(x => x.Rfid == int.Parse(rfid).ToString()).MoldName;
                memorStream = ExportService.ExportErrorState(dic, customData, "ERROR_STATISTIFC_REPORT.xlsx");
                fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_[{int.Parse(rfid)} ({moldName})]이상 통계.xlsx";
            }
            else if (pageName.Equals("RawDataPage"))
            {
                List<RowItem> customData = RowDataService.Select(dic);
                if (customData == null || customData.Count == 0)
                    return Content("<html><head><script>alert('다운로드 데이터가 없습니다.'); history.back();</script></head></html>", "text/html; charset=utf-8");

                moldName = customData.Find(x => x.Rfid == int.Parse(rfid).ToString()).MoldName;
                memorStream = ExportService.ExportRawData(dic, customData, "RAW_DATA_REPORT.xlsx");
                fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_[{int.Parse(rfid)} ({moldName})]RawData 이력.xlsx";
            }

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(memorStream, contentType, fileName);

        }
    }
}
