using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using Framework;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    /// <summary>
    /// Row Data 이력 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RowDataController : ControllerBaseEx
    {
        private readonly ILogger<RowDataController> _logger;

        public RowDataController(ILogger<RowDataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 캐시사용
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public RowDataList Get()
        {
            RowDataList rowdataList = RowDataService.List(base.UserCorpCode);
            return rowdataList;
        }

  
        [HttpPost]
        [Route("search")]
        public JArray Select(HybridDictionary param)
        {
            List<RowItem> lstRowItemS = RowDataService.Select(param);
            JArray json =JArray.FromObject(lstRowItemS);
            return json;
        }

        [HttpGet]
        [Route("selectone")]
        public RowDataList SelectOne(string corpCode, string rackNumber)
        {
            RowDataList lstRowdataS = RowDataService.SelectOne(corpCode, rackNumber);
            return lstRowdataS;
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
