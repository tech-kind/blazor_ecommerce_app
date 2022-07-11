using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlazorECommerceApp.Shared.Entities;
using BlazorECommerceApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

namespace BlazorECommerceApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpGet("filter/{productId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<ActionResult<List<Review>>> FilterByProductIdAsync(int productId)
        {
            return Ok(await _reviewService.FilterByProductIdAsync(productId));
        }

        [Authorize]
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async ValueTask<ActionResult<int>> Post(Review review)
        {
            return Ok(await _reviewService.PostAsync(review, GetUserId()));
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
