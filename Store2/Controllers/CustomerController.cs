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
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly IFunctional _functionalService;

        public CustomerController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Customer> Items = _functionalService.GetList<Customer>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Customer> payload)
        {
            Customer value = payload.value;
            var result = _functionalService.Insert<Customer>(value);
            value = (Customer)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CustomerViewModel> payload)
        {
            CustomerViewModel value = payload.value;
            var result = _functionalService
                .Update<CustomerViewModel, Customer>(value, Convert.ToInt32(value.CustomerId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Customer> payload)
        {
            var result = _functionalService.Delete<Customer>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}