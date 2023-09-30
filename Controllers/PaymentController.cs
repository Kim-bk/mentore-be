using Mentore.Models.DTOs.Requests;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using API.Model.DTOs.Requests;

namespace Mentore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //[AllowAnonymous]
        [HttpPost("vnpay")]
        public async Task<IActionResult> VNPayCheckOut(WorkshopRequest request)
        {
            try
            {
                //var userId = "1228baed-55dc-4dce-8cee-bcaf634ca96a";
                var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var (mesage, isSuccess) = await _paymentService.VNPayCheckOut(request, userId, HttpContext);
                if (!isSuccess)
                    return BadRequest(mesage);

                return Ok(mesage);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpGet("call-back")]
        public async Task<IActionResult> PaymentSuccess()
        {
            var rs = await _paymentService.PaySuccess(Request.Query);
            if (rs)
                return Redirect("http://localhost:8080/payment-success");
         
            return BadRequest("Thanh toán thất bại !");
        }
    }
}