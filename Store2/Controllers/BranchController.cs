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
    [Route("api/Branch")]
    public class BranchController : Controller
    {
        private readonly IFunctional _functionalService;

        public BranchController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            List<Branch> Items = _functionalService.GetList<Branch>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Branch> payload)
        {
            Branch value = payload.value;
            var result = _functionalService.Insert<Branch>(value);
            value = (Branch)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<BranchViewModel> payload)
        {
            BranchViewModel value = payload.value;
            var result = _functionalService
                .Update<BranchViewModel, Branch>(value, Convert.ToInt32(value.BranchId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Branch> payload)
        {
            var result = _functionalService.Delete<Branch>(Convert.ToInt32(payload.key));
            return Ok();

        }

    }
}