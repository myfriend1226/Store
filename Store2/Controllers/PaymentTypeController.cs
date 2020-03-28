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
    [Route("api/PaymentType")]
    public class PaymentTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public PaymentTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<PaymentType> Items = _functionalService.GetList<PaymentType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PaymentType> payload)
        {
            PaymentType value = payload.value;
            var result = _functionalService.Insert<PaymentType>(value);
            value = (PaymentType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PaymentTypeViewModel> payload)
        {
            PaymentTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<PaymentTypeViewModel, PaymentType>(value, Convert.ToInt32(value.PaymentTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PaymentType> payload)
        {
            var result = _functionalService.Delete<PaymentType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}