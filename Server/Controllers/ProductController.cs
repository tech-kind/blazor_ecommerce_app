using System;
using System.Net.Mime;
using BlazorECommerceApp.Server.Services;
using BlazorECommerceApp.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorECommerceApp.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product is null)
            {
                return NotFound("商品が見つかりませんでした。");
            }

            return Ok(product);
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<ActionResult<List<Product>>> GetAll()
            => Ok(await _productService.GetAllAsync());
    }
}
