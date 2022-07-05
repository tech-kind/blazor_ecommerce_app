using System;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BlazorECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeWebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("fulfill")]
        public async ValueTask<IActionResult> FulFill()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
