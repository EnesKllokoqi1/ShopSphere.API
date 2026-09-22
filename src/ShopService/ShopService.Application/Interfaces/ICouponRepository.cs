using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> CreateCouponAsync(Coupon coupon);
        Task<Coupon?> GetCouponByIdAsync(Guid couponId);
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code);
        Task<Coupon?> UpdateCouponAsync(Coupon updatedCoupon, Guid couponId);
        Task<bool> DeleteCouponAsync(Guid couponId);
        Task<IEnumerable<CouponResposneDTO>?> GetAllCouponsAsync(int pageNumber,int pageSize);
        Task<IEnumerable<CouponResposneDTO>?> GetCouponsLinkedToProduct(Guid productId,int pageNumber, int pageSize);
        Task<IEnumerable<ProductSummaryDTO?>> GetProductsLinkedToCoupon(Guid couponId, int pageNumber, int pageSize);
        Task<bool> AddProductToCouponAsync(Guid couponId, Guid productId);
        Task<bool> RemoveProductFromCouponAsync(Guid couponId, Guid productId);
        Task<int> GetUserUsageCountAsync(Guid couponId, Guid userId);
        Task<bool> TryIncrementUsedCountAsync(Guid couponId);
        Task DecrementUsedCountAsync(Guid couponId);
    }
}
