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
    [Route("api/PurchaseType")]
    public class PurchaseTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public PurchaseTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<PurchaseType> Items = _functionalService.GetList<PurchaseType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PurchaseType> payload)
        {
            PurchaseType value = payload.value;
            var result = _functionalService.Insert<PurchaseType>(value);
            value = (PurchaseType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PurchaseTypeViewModel> payload)
        {
            PurchaseTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<PurchaseTypeViewModel, PurchaseType>(value, Convert.ToInt32(value.PurchaseTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PurchaseType> payload)
        {
            var result = _functionalService.Delete<PurchaseType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}