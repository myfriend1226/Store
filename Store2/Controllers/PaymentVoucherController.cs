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
    [Route("api/PaymentVoucher")]
    public class PaymentVoucherController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public PaymentVoucherController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }


        [HttpGet]
        public IActionResult Get()
        {
            List<PaymentVoucher> Items = _functionalService.GetList<PaymentVoucher>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PaymentVoucher> payload)
        {
            PaymentVoucher value = payload.value;
            value.PaymentVoucherName = _numberSequence.GetNumberSequence("PAYVCH");
            var result = _functionalService.Insert<PaymentVoucher>(value);
            value = (PaymentVoucher)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PaymentVoucherViewModel> payload)
        {
            PaymentVoucherViewModel value = payload.value;
            var result = _functionalService
                .Update<PaymentVoucherViewModel, PaymentVoucher>(value, Convert.ToInt32(value.PaymentVoucherId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PaymentVoucher> payload)
        {
            var result = _functionalService.Delete<PaymentVoucher>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}