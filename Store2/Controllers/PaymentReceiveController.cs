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
    [Route("api/PaymentReceive")]
    public class PaymentReceiveController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public PaymentReceiveController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }

        // GET: api/PaymentReceive
        [HttpGet]
        public IActionResult GetPaymentReceive()
        {
            List<PaymentReceive> Items = _functionalService.GetList<PaymentReceive>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PaymentReceive> payload)
        {
            PaymentReceive paymentReceive = payload.value;
            paymentReceive.PaymentReceiveName = _numberSequence.GetNumberSequence("PAYRCV");
            var result = _functionalService.Insert<PaymentReceive>(paymentReceive);
            paymentReceive = (PaymentReceive)result.Data;
            return Ok(paymentReceive);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PaymentReceiveViewModel> payload)
        {
            PaymentReceiveViewModel value = payload.value;
            var result = _functionalService
                .Update<PaymentReceiveViewModel, PaymentReceive>(value, Convert.ToInt32(value.PaymentReceiveId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PaymentReceive> payload)
        {
            var result = _functionalService.Delete<PaymentReceive>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}