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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetAllProducts(pageNumber, pageSize);
            return Ok(products);
        }
        [Authorize]
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
        [Authorize]
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryResponseDTO>>> GetProductCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var categories = await _productService.GetProductCategories(pageNumber,pageSize);
            return Ok(categories);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
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
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{productId:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductDTO updateProductDTO)
        {
            var updatedProduct = await _productService.UpdateProductDto(updateProductDTO, productId);
            if (updatedProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return Ok(updatedProduct);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{productId:guid}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var success = await _productService.DeleteProduct(productId);
            if (!success)
            {
                return NotFound(new { Message = "Product not found." });
            }

            return NoContent();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{productId:guid}/stock")]
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
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetLowStockProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var lowStockProducts = await _productService.GetLowStockProductsAsync(pageNumber,pageSize);
            return Ok(lowStockProducts);
        }
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetFeaturedProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var featuredProducts = await _productService.GetFeaturedProducts(pageNumber, pageSize);
            return Ok(featuredProducts);
        }
        [HttpGet("{productId:guid}/reviews")]
        public async Task<ActionResult<IEnumerable<ProductReviewResponseDTO>>> GetProductReviews([FromRoute] Guid productId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var productReviews = await _productService.GetProductReviews(productId, pageNumber, pageSize);
            return Ok(productReviews);
        }
    }
    public record AdjustStockDTO(int QuantityChange);
}