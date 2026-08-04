using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopService.Application.DTOs;
using ShopService.Application.DTOs.ProductDTOs;

namespace ShopService.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProducts();
        Task<ProductResponseDTO> GetSpecificProduct(Guid guid );
        Task<List<ProductCategoryResponseDTO>> GetProductCategories();
        Task<ProductResponseDTO> CreateProduct(CreateProductDTO createProductDTO);
        Task<ProductResponseDTO> UpdateProductDto(UpdateProductDTO createProductDTO);
        Task<bool> DeleteProduct(Guid guid);
        Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
        Task<List<ProductResponseDTO>> GetLowStockProductsAsync();
        Task<List<ProductResponseDTO>> GetFeaturedProducts();
        Task<ProductReviewResponseDTO> GetProductReviews(Guid guid);
    }
}
