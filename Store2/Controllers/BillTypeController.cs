﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store2.Models;
using Store2.Models.SyncfusionViewModels;
using Store2.Services;

namespace Store2.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/BillType")]
    public class BillTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public BillTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            List<BillType> Items = _functionalService.GetList<BillType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<BillType> payload)
        {
            BillType value = payload.value;
            var result = _functionalService.Insert<BillType>(value);
            value = (BillType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<BillTypeViewModel> payload)
        {
            BillTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<BillTypeViewModel, BillType>(value, Convert.ToInt32(value.BillTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<BillType> payload)
        {
            var result = _functionalService.Delete<BillType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}