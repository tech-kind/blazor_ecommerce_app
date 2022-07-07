using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using BlazorECommerceApp.Server.Services;
using BlazorECommerceApp.Shared.Entities;

namespace BlazorECommerceApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async ValueTask<ActionResult<List<Order>>> GetAll()
        {
            return Ok(await _orderService.GetAllAsync(GetUserId()));
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
