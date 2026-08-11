<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System.Text.RegularExpressions;
using ShopService.Infrastructure.Data;
=======
using Microsoft.Extensions.Configuration;
using ShopService.Application.DTOs.ProductDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
<<<<<<< HEAD
=======
using System.Runtime.InteropServices;
>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class ProductService : IProductService
    {
<<<<<<< HEAD
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> AdjustStockAsync(Guid productId, int quantityChange)
        {
            var product = await GetProductById(productId);
            if (product is null)
            {
                return false;
            }
            product.StockQuantity += quantityChange;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> CreateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Slug))
            {
                product.Slug = GenerateSlug(product.Name);
            }
            bool exists = await _appDbContext.Products
        .AnyAsync(p => p.Sku == product.Sku || p.Slug == product.Slug || p.Name == product.Name);
            if (exists)
            {
                return null;
            }
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
            
        }

        private string GenerateSlug(string name)
        {
            string slug = name.ToLower().Replace(" ", "-").Replace("&", "and");
            return Regex.Replace(slug, "[^a-z0-9-]", "");
=======
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
>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
        }

        public async Task<bool> DeleteProduct(Guid guid)
        {
<<<<<<< HEAD
            var product = await GetProductById(guid);
            if (product is null)
            {   
                return false;
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async  Task<IEnumerable<ProductResponseDTO>> GetFeaturedProducts()
        {
            return await _appDbContext.Products
                .Where(e => e.IsFeatured == true)
                .Select(MapToProductResponseDTO())
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetLowStockProductsAsync()
        {
            return await _appDbContext.Products
         .Where(e => e.StockQuantity <= (e.LowStockThreshold ?? 10))
         .Select(MapToProductResponseDTO())
         .ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            return await _appDbContext.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<ProductCategoryResponseDTO>> GetProductCategories()
        {
            return await _appDbContext.Categories
         .Select(c => new ProductCategoryResponseDTO
         {
             Id = c.Id,
             Name = c.Name,
             Description = c.Description,
             ParentCategoryId = c.ParentCategoryId,
             DisplayOrder = c.DisplayOrder,
             IsActive = c.IsActive,
             CreatedAt = c.CreatedAt,
             UpdatedAt = c.UpdatedAt,
             Products = c.Products.Select(p => new ProductResponseDTO
             {
                 Id = p.Id,
                 Name = p.Name,
                 Slug = p.Slug,
                 Description = p.Description,
                 ShortDescription = p.ShortDescription,
                 Sku = p.Sku,
                 Price = p.Price,
                 CompareAtPrice = p.CompareAtPrice,
                 StockQuantity = p.StockQuantity,
                 LowStockThreshold = p.LowStockThreshold,
                 IsActive = p.IsActive,
                 IsFeatured = p.IsFeatured,
                 Brand = p.Brand,
                 Weight = p.Weight,
                 CreatedAt = p.CreatedAt,
                 UpdatedAt = p.UpdatedAt
             }).ToList()
         })
         .ToListAsync();
        }

        public async Task<IEnumerable<ProductReviewResponseDTO>> GetProductReviews(Guid productId)
        {
            return await _appDbContext.Reviews
                .Where(e => e.ProductId == productId)
                .Select(e => new ProductReviewResponseDTO
            {
                ProductId=e.ProductId,
                ProductName =e.Product.Name,
                ProductSku=e.Product.Sku,
                ReviewId=e.Id,
                Slug=e.Product.Slug,
                Rating=e.Rating,
                Comment=e.Comment,
                ReviewCreatedAt=e.CreatedAt,
            }).ToListAsync();
        }
        public async Task<Product?> UpdateProduct(Product updatedProduct,Guid productId)
        {
            var product = await GetProductById(productId);
            if (product is null)
            {
                return null;
            }
            if (product.Name != updatedProduct.Name)
            {
                product.Slug = GenerateSlug(updatedProduct.Name);
            }
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.ShortDescription = updatedProduct.ShortDescription;
            product.Sku = updatedProduct.Sku;
            product.Price = updatedProduct.Price;
            product.CompareAtPrice = updatedProduct.CompareAtPrice;
            product.StockQuantity = updatedProduct.StockQuantity;
            product.LowStockThreshold = updatedProduct.LowStockThreshold;
            product.IsActive = updatedProduct.IsActive;
            product.IsFeatured = updatedProduct.IsFeatured;
            product.Brand = updatedProduct.Brand;
            product.Weight = updatedProduct.Weight;
            product.CategoryId = updatedProduct.CategoryId;
            await _appDbContext.SaveChangesAsync();
            return product;
        }
        private static Expression<Func<Product, ProductResponseDTO>> MapToProductResponseDTO()
        {
            return e => new ProductResponseDTO
            {
                Id = e.Id,
                Name = e.Name,
                Slug = e.Slug,
                Description = e.Description,
                ShortDescription = e.ShortDescription,
                Sku = e.Sku,
                Price = e.Price,
                CompareAtPrice = e.CompareAtPrice,
                StockQuantity = e.StockQuantity,
                LowStockThreshold = e.LowStockThreshold,
                IsActive = e.IsActive,
                IsFeatured = e.IsFeatured,
                Brand = e.Brand,
                Weight = e.Weight,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            };
=======
            return await _productRepository.DeleteProduct(guid);
>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProducts()
        {
<<<<<<< HEAD
            return await _appDbContext.Products
           .AsNoTracking()
           .Select(MapToProductResponseDTO())
           .ToListAsync();
        }
=======
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

>>>>>>> f13f5f5b2644d0f778cc200d28ea4539e6438f8b
    }
}
