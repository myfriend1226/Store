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
    [Route("api/VendorType")]
    public class VendorTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public VendorTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<VendorType> Items = _functionalService.GetList<VendorType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<VendorType> payload)
        {
            VendorType value = payload.value;
            var result = _functionalService.Insert<VendorType>(value);
            value = (VendorType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<VendorTypeViewModel> payload)
        {
            VendorTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<VendorTypeViewModel, VendorType>(value, Convert.ToInt32(value.VendorTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<VendorType> payload)
        {
            var result = _functionalService.Delete<VendorType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}