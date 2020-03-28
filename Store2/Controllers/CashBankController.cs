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
    [Route("api/CashBank")]
    public class CashBankController : Controller
    {
        private readonly IFunctional _functionalService;

        public CashBankController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<CashBank> Items = _functionalService.GetList<CashBank>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<CashBank> payload)
        {
            CashBank value = payload.value;
            var result = _functionalService.Insert<CashBank>(value);
            value = (CashBank)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CashBankViewModel> payload)
        {
            CashBankViewModel value = payload.value;
            var result = _functionalService
                .Update<CashBankViewModel, CashBank>(value, Convert.ToInt32(value.CashBankId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<CashBank> payload)
        {
            var result = _functionalService.Delete<CashBank>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}