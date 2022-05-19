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

namespace Web.Controllers
{
    ///
    [ApiController]
    [Route("api/[controller]")]
    public class RackLocationController : ControllerBaseEx
    {
        private readonly ILogger<RackLocationController > _logger;
        public RackLocationController (ILogger<RackLocationController > logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DB정보를 메모리에 저장 하고 가져오는 함수 (캐시사용)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getcache")]
        public RackList Get()
        {
            RackList lstRackS = RackService.List(base.UserCorpCode);
            return lstRackS;
        }

        /// <summary>
        /// Barcode 전체 리스트 
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet]
      
        public List<KeyValuePair<string, List<RackEntity>>> Select()
        {
            List<KeyValuePair<string, List<RackEntity>>> listObj = RackService.Select().ToList();
            return listObj;
        }

        [HttpGet]
        [Route("moldinfo")]
        public List<MoldPosEntity> SelectOne( )
        {
            List<MoldPosEntity> lstRackS = RackService.SelectOne();
            return lstRackS;
        }


        [HttpGet]
        [Route("statuses")]
        public Dictionary<string, MachineEntity> SelectMachine()
        {
            Dictionary<string, MachineEntity> dicRackS = RackService.SelectMachine();
            return dicRackS;
        }

        [HttpGet]
        [Route("location")]
        public Dictionary<string, LocationEntity> SelectLocation()
        {
            Dictionary<string, LocationEntity> dicLocationS = RackService.SelectLocation();
            return dicLocationS;
        }
    }
}
