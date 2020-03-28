using System;
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
    [Route("api/CustomerType")]
    public class CustomerTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public CustomerTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<CustomerType> Items = _functionalService.GetList<CustomerType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<CustomerType> payload)
        {
            CustomerType value = payload.value;
            var result = _functionalService.Insert<CustomerType>(value);
            value = (CustomerType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CustomerTypeViewModel> payload)
        {
            CustomerTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<CustomerTypeViewModel, CustomerType>(value, Convert.ToInt32(value.CustomerTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<CustomerType> payload)
        {
            var result = _functionalService.Delete<CustomerType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}