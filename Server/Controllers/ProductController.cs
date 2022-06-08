using System;
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

        [HttpGet]
        public async ValueTask<ActionResult<List<Product>>> GetAll()
            => Ok(await _productService.GetAllAsync());
    }
}
