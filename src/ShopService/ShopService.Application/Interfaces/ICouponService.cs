using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICouponService
    {
        Task<CouponResposneDTO?> CreateCouponAsync(CreateCouponDTO createCouponDTO);
        Task<CouponResposneDTO?> UpdateCouponAsync(UpdateCouponDTO updateCouponDTO, Guid couponId);
        Task<CouponResposneDTO?> GetCouponByIdAsync(Guid couponId);
        Task<CouponResposneDTO?> GetCouponByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> DeleteCouponAsync(Guid couponId);
        Task<IEnumerable<CouponResposneDTO>?> GetAllCouponsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<CouponResposneDTO>?> GetCouponsLinkedToProduct(Guid productId, int pageNumber, int pageSize);
        Task<IEnumerable<ProductSummaryDTO?>> GetProductsLinkedToCoupon(Guid couponId, int pageNumber, int pageSize);
        Task<bool> AddProductToCouponAsync(Guid couponId, Guid productId);
        Task<bool> RemoveProductFromCouponAsync(Guid couponId, Guid productId);
        Task<bool> TryIncrementUsedCountAsync(Guid couponId);
        Task<bool> DecrementUsedCountAsync(Guid couponId);
        Task<int> GetUserUsageCountAsync(Guid couponId, Guid userId);
    }
}

