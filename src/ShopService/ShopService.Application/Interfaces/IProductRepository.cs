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
        Task<IEnumerable<ProductResponseDTO>> GetAllProducts(int pageNumber,int pageSize);
        Task<bool> DeleteProduct(Guid guid);
        Task<Product?> UpdateProduct(Product updatedProduct, Guid productId);
        Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
        Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories(int pageNumber,int pageSize);
        Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts(int pageNumber, int pageSize);
        Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid productId, int pageNumber, int pageSize);
    }
}