using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RavePay.Payment.Helpers;
using RavePay.Payment.Payments;
using RavePay.Web.Models;
using RavePay.Web.Models.Services;

namespace RavePay.Web.Controllers
{



    public class HomeController : Controller
    {
        
        private readonly IOrderService _service;
        public HomeController(IOrderService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("order/success")]
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        

        [Route("payment/verify")]
        public async Task<IActionResult> verify()
        {
            var tranxRef = Request.Query["tx_ref"];
            var transactionId = Request.Query["transaction_id"];
            var transStatus = Request.Query["status"];

            if (!transStatus.Equals("successful") || string.IsNullOrWhiteSpace(transactionId))
            {
                return RedirectToAction("FailedPayment", "Error", new { area = "" });
            }

            var transaction_id = int.Parse(transactionId);

            var raveScretKey = RaveConstant.SANDBOX_KEY;
            var raveAPI = new RavePayment(raveScretKey);

            var response = await raveAPI.VerifyTransaction(transaction_id);

            if (response.status == "success")
            {
                if (response.data.status == "successful")
                {
                    var update = await _service.UpdateOrder(tranxRef, true);
                    if (update)
                    {
                        return RedirectToAction("PaymentSuccess", "Home", new { area = "" });
                    }
                }
            }

            return RedirectToAction("FailedPayment", "Error", new { area = "" });
        }


        [Route("inittrans")]
        public async Task<IActionResult> TransactionInit([FromBody] CartModel model)
        {
            var raveScretKey = RaveConstant.SANDBOX_KEY;
            var raveAPI = new RavePayment(raveScretKey);

            var callback_url = "http://localhost:51220/payment/verify";
            var trx_ref = DateTime.Now.Ticks.ToString();
            var orderTotal = Convert.ToDecimal(model.amount);
            var customerId = await _service.AddCustomer(model);

            var orderRef = await _service.AddOrder(trx_ref, customerId, orderTotal);

            var initResponse = await raveAPI.InitializeTransaction(orderRef, model.amount, callback_url,
                 model.email,model.phone,model.customerName);

            if (!initResponse.status.Equals("success")) return Json(new { status = "error" });

            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            return Json(initResponse);


        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
