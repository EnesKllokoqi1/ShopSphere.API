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
<<<<<<< HEAD
        Task<IEnumerable<ProductResponseDTO>> GetAllProducts();
        Task<ProductResponseDTO?> GetSpecificProduct(Guid guid );
        Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories();
        Task<ProductResponseDTO?> CreateProduct(CreateProductDTO createProductDTO);
        Task<ProductResponseDTO?> UpdateProductDto(UpdateProductDTO updateProductDTO,Guid productId);
        Task<bool> DeleteProduct(Guid guid);
        Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
        Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync();
        Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts();
        Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid guid);
=======
         Task<IEnumerable<ProductResponseDTO>> GetAllProducts();
         Task<ProductResponseDTO?> GetSpecificProduct(Guid guid );
         Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories();
         Task<ProductResponseDTO?> CreateProduct(CreateProductDTO createProductDTO);
         Task<ProductResponseDTO?> UpdateProductDto(UpdateProductDTO updateProductDTO,Guid productId);
         Task<bool> DeleteProduct(Guid guid);
         Task<bool> AdjustStockAsync(Guid productId, int quantityChange);
         Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync();
         Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts();
         Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid guid);
>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
    }
}
