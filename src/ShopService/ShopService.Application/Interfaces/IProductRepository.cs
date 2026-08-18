using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductById(Guid productId);
        Task<Product?> CreateProduct(Product product);
        Task<IEnumerable<ProductResponseDTO>> GetAllProducts();
        Task<bool> DeleteProduct(Guid guid);
        Task<Product?> UpdateProduct(Product updatedProduct, Guid productId);
        Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
        Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories();
        Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync();
        Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts();
        Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid productId);
    }
}