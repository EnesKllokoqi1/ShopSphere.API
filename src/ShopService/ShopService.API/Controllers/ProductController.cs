using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;

namespace ShopService.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("{productId:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductById([FromRoute] Guid productId)
        {
            var product = await _productService.GetSpecificProduct(productId);
            if (product is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(product);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryResponseDTO>>> GetProductCategories()
        {
            var categories = await _productService.GetProductCategories();
            return Ok(categories);
        }
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            var createdProduct = await _productService.CreateProduct(createProductDTO);
            if (createdProduct is null)
            {
                return Conflict(new { Message = "Product already exists." });
            }

            return CreatedAtAction(
                nameof(GetProductById),
                new { productId = createdProduct.Id },
                createdProduct
            );
        }
        [HttpPut("{productId:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductDTO updateProductDTO)
        {
            var updatedProduct = await _productService.UpdateProductDto(updateProductDTO, productId);
            if (updatedProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(updatedProduct);
        }
        [HttpDelete("{productId:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var success = await _productService.DeleteProduct(productId);
            if (!success)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return NoContent();
        }
        [HttpPut("{productId:guid}/stock")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AdjustStock([FromRoute] Guid productId, [FromBody] AdjustStockDTO adjustStockDto)
        {
            var success = await _productService.AdjustStockAsync(productId, adjustStockDto.QuantityChange);
            if (!success)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(new { Message = "Stock successfully adjusted." });
        }
        [HttpGet("low-stock")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetLowStockProducts()
        {
            var lowStockProducts = await _productService.GetLowStockProductsAsync();
            return Ok(lowStockProducts);
        }
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetFeaturedProducts()
        {
            var featuredProducts = await _productService.GetFeaturedProducts();
            return Ok(featuredProducts);
        }
        [HttpGet("{productId:guid}/reviews")]
        public async Task<ActionResult<IEnumerable<ProductReviewResponseDTO>>> GetProductReviews([FromRoute] Guid productId)
        {
            var productReviews = await _productService.GetProductReviews(productId);
            return Ok(productReviews);
        }
    }
    public record AdjustStockDTO(int QuantityChange);
}