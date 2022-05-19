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
    [ApiController]
    [Route("api/[controller]")]
    public class TempController : ControllerBaseEx
    {
        private readonly ILogger<TempController> _logger;

        public TempController(ILogger<TempController> logger)
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

        /// <summary>
        /// 전체 리스트 
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet]
        public TempList Select()
        {
            TempList lstTempS = TempService.Select();
            return lstTempS;
        }

        /// <summary>
        /// 선택한 리스트
        /// </summary>
        /// <param name="rackNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("selectone")]
        public TempList SelectOne(string rackNumber)
        {
            TempList lstTempS= TempService.SelectOne(UserCorpCode, rackNumber);
            return lstTempS;
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
