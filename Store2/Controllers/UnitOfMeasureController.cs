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
    [Route("api/UnitOfMeasure")]
    public class UnitOfMeasureController : Controller
    {
        private readonly IFunctional _functionalService;

        public UnitOfMeasureController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<UnitOfMeasure> Items = _functionalService.GetList<UnitOfMeasure>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<UnitOfMeasure> payload)
        {
            UnitOfMeasure value = payload.value;
            var result = _functionalService.Insert<UnitOfMeasure>(value);
            value = (UnitOfMeasure)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<UnitOfMeasureViewModel> payload)
        {
            UnitOfMeasureViewModel value = payload.value;
            var result = _functionalService
                .Update<UnitOfMeasureViewModel, UnitOfMeasure>(value, Convert.ToInt32(value.UnitOfMeasureId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<UnitOfMeasure> payload)
        {
            var result = _functionalService.Delete<UnitOfMeasure>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}