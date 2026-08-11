using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;

namespace ShopService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("get-all-products")]
        public async Task<ActionResult> GetAllProducts()
        {
            throw new NotImplementedException();
        }
        [HttpGet("get-specific-product/{productId:Guid}")]
        public async Task<ActionResult> GetProductById([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }
        [HttpGet("get-product-categories")] 
        public async Task<ActionResult> GetProductCategories()
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-product/{productId:Guid}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductDTO updateProductDTO)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete-product/{productId:Guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{productId:guid}/adjust-stock")]
        public async Task<ActionResult> AdjustStock([FromRoute] Guid productId,[FromBody] int quantityChange)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-lowstock-products")]
        public async Task<ActionResult> GetLowStockProducts()
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-featured-products")]
        public async Task<ActionResult> GetFeaturedProducts()
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{productId:Guid}/product-reviews")]
        public async Task<ActionResult> GetProductReviews([FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }

    }
}
