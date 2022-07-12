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
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<ActionResult<Review>> Get(int id)
        {
            var review = await _reviewService.GetAsync(id);
            if (review is null)
            {
                return NotFound("レビューが見つかりませんでした。");
            }

            return Ok(review);
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

        [Authorize]
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<ActionResult> Put(Review review)
        {
            var statusCode = await _reviewService.PutAsync(review, GetUserId());

            return statusCode switch
            {
                StatusCodes.Status400BadRequest => BadRequest(),
                StatusCodes.Status404NotFound => NotFound("レビューが見つかりませんでした。"),
                _ => NoContent()
            };
        }

        [Authorize]
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<ActionResult> Delete(int id)
        {
            var statusCode = await _reviewService.DeleteAsync(id, GetUserId());

            return statusCode switch
            {
                StatusCodes.Status400BadRequest => BadRequest(),
                StatusCodes.Status404NotFound => NotFound("レビューが見つかりませんでした。"),
                _ => NoContent()
            };
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
