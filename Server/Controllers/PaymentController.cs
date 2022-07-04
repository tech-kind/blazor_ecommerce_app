using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using BlazorECommerceApp.Server.Services;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IIPaymentService _paymentService;

        public PaymentController(IIPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-checkout-session")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<ActionResult<string>> CreateCheckoutSession([FromBody] List<Cart> carts)
        {
            var sessionUrl = await _paymentService.CreateCheckoutSessionAsync(carts, GetUserId());

            return Ok(sessionUrl);
        }

        private string GetUserId()
            => User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
