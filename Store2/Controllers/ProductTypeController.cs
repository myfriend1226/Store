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
    [Route("api/ProductType")]
    public class ProductTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public ProductTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<ProductType> Items = _functionalService.GetList<ProductType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<ProductType> payload)
        {
            ProductType value = payload.value;
            var result = _functionalService.Insert<ProductType>(value);
            value = (ProductType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ProductTypeViewModel> payload)
        {
            ProductTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<ProductTypeViewModel, ProductType>(value, Convert.ToInt32(value.ProductTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<ProductType> payload)
        {
            var result = _functionalService.Delete<ProductType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}