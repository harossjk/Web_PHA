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


namespace Web.Controllers
{
    /// <summary>
    /// 공정 지수 통계 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessStateController : ControllerBaseEx
    {
        private readonly ILogger<ProcessStateController> _logger;

        public ProcessStateController(ILogger<ProcessStateController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 캐시사용
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public ProcessList Get()
        {
            ProcessList tempList = ProcessStateService.List(base.UserCorpCode);
            return tempList;
        }

        /// <summary>
        /// 전체 리스트 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MoldList Select()
        {
            MoldList lstTempS = ProcessStateService.Select();
            return lstTempS;
        }

        /// <summary>
        /// 선택한 리스트
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("selectone")]
        public ProcessList SelectOne(string rfid)
        {
            ProcessList lstTempS = ProcessStateService.SelectOne(UserCorpCode, rfid);
            return lstTempS;
        }

        /// <summary>
        /// 압력적분 리스트
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getInpSearch")]
        public JObject SelectInpSearch(HybridDictionary dic)
        {
            ProcessEntitiy customData = ProcessStateService.GetIntegralPressureSearch(dic);

            JObject json = (JObject)JToken.FromObject(customData);
            return json;
        }

        [HttpPut]
        public IActionResult Create(HybridDictionary dic)
        {
            RefineParam(dic, false);

            try
            {
                var result = 0;//MenuService.Insert(dic);
                return result > 0 ? Ok() : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Update(ParameterDictionary dic)
        {
            RefineParam(dic);

            if (!dic.ContainsKey("menuId"))
                throw new ApplicationException("menuId가 없습니다.");

            try
            {
                var result = 0; //MenuService.Update(dic);
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
