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
    [Route("api/ShipmentType")]
    public class ShipmentTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public ShipmentTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<ShipmentType> Items = _functionalService.GetList<ShipmentType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<ShipmentType> payload)
        {
            ShipmentType value = payload.value;
            var result = _functionalService.Insert<ShipmentType>(value);
            value = (ShipmentType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ShipmentTypeViewModel> payload)
        {
            ShipmentTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<ShipmentTypeViewModel, ShipmentType>(value, Convert.ToInt32(value.ShipmentTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<ShipmentType> payload)
        {
            var result = _functionalService.Delete<ShipmentType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}