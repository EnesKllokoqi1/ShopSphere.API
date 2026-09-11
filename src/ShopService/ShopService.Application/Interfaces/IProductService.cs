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
        Task<IEnumerable<ProductResponseDTO>> GetAllProducts(int pageNumber,int pageSize);
        Task<ProductResponseDTO?> GetSpecificProduct(Guid guid);
        Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories(int pageNumber,int pageSize);
        Task<ProductResponseDTO?> CreateProduct(CreateProductDTO createProductDTO);
        Task<ProductResponseDTO?> UpdateProductDto(UpdateProductDTO updateProductDTO, Guid productId);
        Task<bool> DeleteProduct(Guid guid);
        Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
        Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts(int pageNumber, int pageSize);
        Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid guid, int pageNumber, int pageSize);
    }
}