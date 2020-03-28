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
    [Route("api/Warehouse")]
    public class WarehouseController : Controller
    {
        private readonly IFunctional _functionalService;

        public WarehouseController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Warehouse> Items = _functionalService.GetList<Warehouse>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Warehouse> payload)
        {
            Warehouse value = payload.value;
            var result = _functionalService.Insert<Warehouse>(value);
            value = (Warehouse)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<WarehouseViewModel> payload)
        {
            WarehouseViewModel value = payload.value;
            var result = _functionalService
                .Update<WarehouseViewModel, Warehouse>(value, Convert.ToInt32(value.WarehouseId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Warehouse> payload)
        {
            var result = _functionalService.Delete<Warehouse>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}