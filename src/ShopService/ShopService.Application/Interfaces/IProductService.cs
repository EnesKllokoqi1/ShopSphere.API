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
    }
}
