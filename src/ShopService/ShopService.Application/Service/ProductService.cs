using Microsoft.Extensions.Configuration;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }
        public async Task<bool> AdjustStockAsync(Guid productId, int quantityChange)
        {
           return await _productRepository.AdjustStockAsync(productId, quantityChange);
        }

        public async Task<ProductResponseDTO?> CreateProduct(CreateProductDTO createProductDTO)
        {
            var product = new Product
            {
                Name = createProductDTO.Name,
                Description = createProductDTO.Description,
                ShortDescription = createProductDTO.ShortDescription,
                Sku = createProductDTO.Sku,
                Price = createProductDTO.Price,
                CompareAtPrice = createProductDTO.CompareAtPrice,
                StockQuantity = createProductDTO.StockQuantity,
                LowStockThreshold = createProductDTO.LowStockThreshold,
                IsActive = createProductDTO.IsActive,
                IsFeatured = createProductDTO.IsFeatured,
                Brand = createProductDTO.Brand,
                CategoryId = createProductDTO.CategoryId,
            };
            var product1 = await _productRepository.CreateProduct(product);
            if (product1 is null)
            {
                return null;
            }
            return MapToDTO(product1);
        }

        public async Task<bool> DeleteProduct(Guid guid)
        {
            return await _productRepository.DeleteProduct(guid);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts()
        {
            var featuredProducts = await _productRepository.GetFeaturedProducts();
            return featuredProducts;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync()
        {
            var lowStockProducts = await _productRepository.GetLowStockProductsAsync();
            return lowStockProducts;
        }

        public async Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories()
        {
            var productCategories = await _productRepository.GetProductCategories();
            return productCategories;
        }

        public async Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid guid)
        {
            var productReviews = await _productRepository.GetProductReviews(guid);
            return productReviews;
        }

        public async Task<ProductResponseDTO?> GetSpecificProduct(Guid guid)
        {
            var specificProduct = await _productRepository.GetProductById(guid);
            if (specificProduct is null)
            {
                return null;
            }
            return MapToDTO(specificProduct);
        }

        public async Task<ProductResponseDTO?> UpdateProductDto(UpdateProductDTO updateProductDTO, Guid productId)
        {
            
                var product = new Product
            {
                Name = updateProductDTO.Name,
                Description = updateProductDTO.Description,
                ShortDescription = updateProductDTO.ShortDescription,
                Sku = updateProductDTO.Sku,
                Price = updateProductDTO.Price,
                CompareAtPrice = updateProductDTO.CompareAtPrice,
                StockQuantity = updateProductDTO.StockQuantity,
                LowStockThreshold = updateProductDTO.LowStockThreshold,
                IsActive = updateProductDTO.IsActive,
                IsFeatured = updateProductDTO.IsFeatured,
                Brand = updateProductDTO.Brand,
                CategoryId = updateProductDTO.CategoryId,
            };
            var updatedProduct = await _productRepository.UpdateProduct(product,productId);
            if (updatedProduct is null)
            {
                return null;
            }
            return MapToDTO(updatedProduct);
        }
        public static ProductResponseDTO MapToDTO(Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                Sku = product.Sku,
                Price = product.Price,
                CompareAtPrice = product.CompareAtPrice,
                StockQuantity = product.StockQuantity,
                LowStockThreshold = product.LowStockThreshold,
                IsActive = product.IsActive,
                IsFeatured = product.IsFeatured,
                Brand = product.Brand,
                Weight = product.Weight,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

    }
}
