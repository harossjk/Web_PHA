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

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Web.Controllers
{
    /// <summary>
    /// 데이터 모니터링
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataMonitorController : ControllerBaseEx
    {
        private readonly ILogger<DataMonitorController> _logger;

        public DataMonitorController(ILogger<DataMonitorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 캐시사용
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public MonitorList Get()
        {
            MonitorList lstDataMonitor = DataMonitorService.List(base.UserCorpCode);
            return lstDataMonitor;
        }

        /// <summary>
        /// 센서가 달려있는 사출기 리스트 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("molddata")]
        public MonitorSelectList Select()
        {
            MonitorSelectList lstMonitorS = DataMonitorService.Select();
            
            return new MonitorSelectList(lstMonitorS);
        }

        /// <summary>
        /// 사출기 선택
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="machineType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("selectone")]
        public MonitorList SelectOne(string cycleNo, string machineId,string machineType)
        {
            try
            {
                //채널 리스트 가져오기 
                List<ChannelEntity> lstChS =  DataMonitorService.SelectChList(machineId);

                //압력, 온도 최근 Cycle기준으로 들고옴
                MonitorList lstPressure = DataMonitorService.GetPressure( cycleNo, machineId, machineType);
                MonitorList lstTemperture = DataMonitorService.GetTemperture(cycleNo, machineId, machineType);

                List<DataMonitorEntity> lstOriDataS = new List<DataMonitorEntity>();
                lstOriDataS.AddRange(lstPressure);
                lstOriDataS.AddRange(lstTemperture);

                List<DataMonitorEntity> lstRedeData = Utility.DataReassembly(Utility.DataCleanup(lstChS, lstOriDataS));
                lstRedeData = Utility.ColorReassembly(lstRedeData, lstOriDataS);
                return new MonitorList(lstRedeData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                List<DataMonitorEntity> lstData = new List<DataMonitorEntity>();
                return new MonitorList(lstData);
            }
        }

        /// <summary>
        /// 압력적분 리스트
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getInp")]
        public MonitorInplList SelectInp(HybridDictionary dic)
        {
            try
            {
                MonitorInplList lstInpS = DataMonitorService.GetIntegralPressure(dic);
                return new MonitorInplList(lstInpS);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MonitorInplList();
            }
        }

        [HttpGet]
        [Route("getonedetail")]
        public JObject SelectOneDetail(string machineId, string machineType,string cycleNo)
        {
            if (machineId == null || machineType == null)
                return null;

            DetailCustomEntitiy customData = DataMonitorService.GetSelectDetail(machineId, machineType, cycleNo);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            //color가 null이 아닌 데이터 추출 
            customData.Values = customData.Values.Where(x => x.Color != null).ToList();

            string forecastJson = System.Text.Json.JsonSerializer.Serialize<DetailCustomEntitiy>(customData, options);
            JObject json = JObject.Parse(forecastJson);
            return json;
        }

        /// <summary>
        /// ALARM 리스트
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("alarm")]
        public List<AlarmEntitiy> SelectAlarm()
        {
            List<AlarmEntitiy> lstAlarmS = DataMonitorService.GetSelectAlarm();
            return lstAlarmS;
        }

        [HttpPut]
        public IActionResult Create(HybridDictionary dic)
        {
            RefineParam(dic, false);

            try
            {
                var result = 0;
                return result > 0 ? Ok() : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Update(HybridDictionary dic)
        {
            try
            {
                var result = DataMonitorService.Update(dic);
                return result > 0 ? Ok() : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(List<HybridDictionary> list)
        {
            HybridDictionary param = new HybridDictionary();
            param.Add("xmldoc", ToXDoc(list));

            RefineParam(param, false);

            try
            {
                var result = 0;//MenuService.DeleteXml(param);

                return result > 0 ? Ok() : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
 