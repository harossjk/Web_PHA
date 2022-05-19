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
    /// 이상 통계
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorStateController : ControllerBaseEx
    {
        private readonly ILogger<ErrorStateController> _logger;

        public ErrorStateController(ILogger<ErrorStateController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 캐시사용
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public ErrorList Get()
        {
            ErrorList tempList = ErrorStateService.List(base.UserCorpCode);
            return tempList;
        }

        /// <summary>
        /// 전체 리스트 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ErrorList Select()
        {
            ErrorList lstTempS = ErrorStateService.Select();
            return lstTempS;
        }

        /// <summary>
        /// 선택한 리스트
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("selectone")]
        public ErrorList SelectOne(string rackNumber)
        {
            ErrorList lstTempS = ErrorStateService.SelectOne(UserCorpCode, rackNumber);
            return lstTempS;
        }

        /// <summary>
        /// 이상통계 리스트
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("getErrSearch")]
        public JObject SelectErrSearch(HybridDictionary dic)
        {
            List<ErrorEntity> dbDataS = ErrorStateService.GetErrorSearch(dic);
            var customData = dbDataS.GroupBy(x => x.Rfid).ToDictionary(key => key.Key, value => value.GroupBy(x => x.MachineId).ToDictionary(key => key.Key, value => value.Select(x => x)));
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
