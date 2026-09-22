using Microsoft.EntityFrameworkCore;
using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _appDbContext;
        public CouponRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> AddProductToCouponAsync(Guid couponId, Guid productId)
        {
            var result = await GetCouponByIdAsync(couponId);
            if (result is null)
            {
                return false;
            }
            if (result.products.Any(p => p.Id == productId))
            {
                return false; 
            }
            var product = await _appDbContext.Products.FirstOrDefaultAsync(e => e.Id == productId);
            if (product is null)
            {
                return false;
            }
            result.products.Add(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _appDbContext.Coupons.AnyAsync(e => e.Code == code);
        }

        public async Task<Coupon?> CreateCouponAsync(Coupon coupon)
        {
            var check = await CodeExistsAsync(coupon.Code);
            if (check)
            {
                return null;
            }
            _appDbContext.Coupons.Add(coupon);
            await _appDbContext.SaveChangesAsync();
            return coupon;
        }

        public async Task DecrementUsedCountAsync(Guid couponId)
        {
            var coupon = await _appDbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
            if (coupon is not null && coupon.UsedCount > 0)
            {
                coupon.UsedCount--;
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteCouponAsync(Guid couponId)
        {
            var result = await GetCouponByIdAsync(couponId);
            if (result is null)
            {
                return false;
            }
            _appDbContext.Coupons.Remove(result);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CouponResposneDTO>?> GetAllCouponsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _appDbContext
                .Coupons
                .AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToCouponResponseDTO())
                .ToListAsync();
        }
        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            var result = await _appDbContext.Coupons.FirstOrDefaultAsync(e => e.Code == code);
            return result;
        }

        public async Task<Coupon?> GetCouponByIdAsync(Guid couponId)
        {
            var result = await _appDbContext
                .Coupons
                .Include(e=>e.products)
                .FirstOrDefaultAsync(e=>e.Id==couponId);
            return result;
        }

        public async Task<IEnumerable<CouponResposneDTO>?> GetCouponsLinkedToProduct(Guid productId, int pageNumber, int pageSize)
        {
         return await _appDbContext
        .Coupons
        .AsNoTracking()
        .Where(c => c.products.Any(p => p.Id == productId))
        .OrderByDescending(c => c.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(MapToCouponResponseDTO())
        .ToListAsync();
        }

        public async Task<IEnumerable<ProductSummaryDTO>?> GetProductsLinkedToCoupon(Guid couponId, int pageNumber, int pageSize)
        {
         return await _appDbContext
        .Products
        .AsNoTracking()
        .Where(e => e.Coupons.Any(c => c.Id == couponId))
        .OrderByDescending(c => c.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(MapToProductSummaryDTO())
        .ToListAsync();
        }

        public async Task<int> GetUserUsageCountAsync(Guid couponId, Guid userId)
        {
            return await _appDbContext
                 .Orders
                 .CountAsync(e => e.CouponId == couponId && e.UserId == userId);
        }

        public async Task<bool> RemoveProductFromCouponAsync(Guid couponId, Guid productId)
        {
            var coupon = await _appDbContext.Coupons
           .Include(c => c.products)
           .FirstOrDefaultAsync(c => c.Id == couponId);
            if (coupon is null)
            {
                return false;
            }
            var product = coupon.products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
            {
                return false;
            }
            coupon.products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> TryIncrementUsedCountAsync(Guid couponId)
        {
            var coupon = await GetCouponByIdAsync(couponId);
            if (coupon is null)
            {
                return false;
            }
            if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses.Value)
            {
                return false; 
            }
            coupon.UsedCount++;
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Coupon?> UpdateCouponAsync(Coupon updatedCoupon, Guid couponId)
        {
            var existing = await GetCouponByIdAsync(couponId);

            if (existing is null)
            {
                return null;
            }

            existing.Code = updatedCoupon.Code;
            existing.Name = updatedCoupon.Name;
            existing.Description = updatedCoupon.Description;
            existing.DiscountType = updatedCoupon.DiscountType;
            existing.DiscountValue = updatedCoupon.DiscountValue;
            existing.MinOrderAmount = updatedCoupon.MinOrderAmount;
            existing.MaxDiscountAmount = updatedCoupon.MaxDiscountAmount;
            existing.MaxUses = updatedCoupon.MaxUses;
            existing.MaxUsesPerUser = updatedCoupon.MaxUsesPerUser;
            existing.StartDate = updatedCoupon.StartDate;
            existing.EndDate = updatedCoupon.EndDate;
            existing.IsActive = updatedCoupon.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            return existing;        
        }
        private static Expression<Func<Coupon, CouponResposneDTO>> MapToCouponResponseDTO()
        {
            return coupon => new CouponResposneDTO
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Name = coupon.Name,
                Description = coupon.Description,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MinOrderAmount = coupon.MinOrderAmount,
                MaxDiscountAmount = coupon.MaxDiscountAmount,
                MaxUses = coupon.MaxUses,
                MaxUsesPerUser = coupon.MaxUsesPerUser,
                UsedCount = coupon.UsedCount,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate,
                IsActive = coupon.IsActive,
                CreatedAt = coupon.CreatedAt,
                UpdatedAt = coupon.UpdatedAt
            };
        }

        private static Expression<Func<Product, ProductSummaryDTO>> MapToProductSummaryDTO()
        {
            return product => new ProductSummaryDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Sku = product.Sku,
                IsActive = product.IsActive
            };
        }
    }
}
