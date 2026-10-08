using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        public async Task<bool> AddProductToCouponAsync(Guid couponId, Guid productId)
        {
            return await _couponRepository.AddProductToCouponAsync(couponId, productId);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _couponRepository.CodeExistsAsync(code);
        }

        public async Task<CouponResponseDTO?> CreateCouponAsync(CreateCouponDTO createCouponDTO)
        {
            var result = MapCouponCreateDto(createCouponDTO);
            var coupon = await _couponRepository.CreateCouponAsync(result);
            if (coupon is null)
            {
                return null;
            }
            var createdCoupon = await _couponRepository.GetCouponByIdAsync(coupon.Id);
            if (createdCoupon is null)
            {
                return null;
            }
            return MapCouponToResponseDto(createdCoupon);
        }

        public async Task<bool> DecrementUsedCountAsync(Guid couponId)
        {
            return await _couponRepository.DecrementUsedCountAsync(couponId);
        }       

        public async Task<bool> DeleteCouponAsync(Guid couponId)
        {
            return await _couponRepository.DeleteCouponAsync(couponId);
        }

        public async Task<IEnumerable<CouponResponseDTO>?> GetAllCouponsAsync(int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            return await _couponRepository.GetAllCouponsAsync(pageNumber, pageSize);
        }

        public async Task<CouponResponseDTO?> GetCouponByCodeAsync(string code)
        {
            var result = await _couponRepository.GetCouponByCodeAsync(code);
            if (result is null)
            {
                return null;
            }
            return MapCouponToResponseDto(result);
        }

        public async Task<CouponResponseDTO?> GetCouponByIdAsync(Guid couponId)
        {
            var coupon = await _couponRepository.GetCouponByIdAsync(couponId);
            if (coupon is null)
            {
                return null;
            }
            return MapCouponToResponseDto(coupon);
        }

        public async Task<IEnumerable<CouponResponseDTO>> GetCouponsLinkedToProduct(Guid productId, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            return await _couponRepository.GetCouponsLinkedToProduct(productId, pageNumber, pageSize);
        }

        public async Task<IEnumerable<ProductSummaryDTO>> GetProductsLinkedToCoupon(Guid couponId, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            return await _couponRepository.GetProductsLinkedToCoupon(couponId, pageNumber, pageSize);
        }

        public async Task<int> GetUserUsageCountAsync(Guid couponId, Guid userId)
        {
            return await _couponRepository.GetUserUsageCountAsync(couponId, userId);
        }

        public async Task<bool> RemoveProductFromCouponAsync(Guid couponId, Guid productId)
        {
            return await _couponRepository.RemoveProductFromCouponAsync(couponId, productId);
        }

        public async Task<bool> TryIncrementUsedCountAsync(Guid couponId)
        {
            return await _couponRepository.TryIncrementUsedCountAsync(couponId);
        }

        public async Task<CouponResponseDTO?> UpdateCouponAsync(UpdateCouponDTO updateCouponDTO, Guid couponId)
        {
            var existing = await _couponRepository.GetCouponByIdAsync(couponId);
            if (existing is null)
                return null;
            var newCode = updateCouponDTO.Code?.Trim().ToUpperInvariant();

            if (!string.IsNullOrWhiteSpace(newCode)
            && newCode != existing.Code
            && await _couponRepository.CodeExistsAsync(newCode))
            {
                return null;
            }
            var updated = new Coupon
            {
                Code = newCode ?? existing.Code,
                Name = updateCouponDTO.Name?.Trim() ?? existing.Name,
                Description = updateCouponDTO.Description?.Trim() ?? existing.Description,
                DiscountType = updateCouponDTO.DiscountType ?? existing.DiscountType,
                DiscountValue = updateCouponDTO.DiscountValue ?? existing.DiscountValue,
                MinOrderAmount = updateCouponDTO.MinOrderAmount ?? existing.MinOrderAmount,
                MaxDiscountAmount = updateCouponDTO.MaxDiscountAmount ?? existing.MaxDiscountAmount,
                MaxUses = updateCouponDTO.MaxUses ?? existing.MaxUses,
                MaxUsesPerUser = updateCouponDTO.MaxUsesPerUser ?? existing.MaxUsesPerUser,
                StartDate = updateCouponDTO.StartDate ?? existing.StartDate,
                EndDate = updateCouponDTO.EndDate ?? existing.EndDate,
                IsActive = updateCouponDTO.IsActive ?? existing.IsActive
            };
            var result = await _couponRepository.UpdateCouponAsync(updated, couponId);

            return result is null ? null : MapCouponToResponseDto(result);
        }
        private Coupon MapCouponCreateDto(CreateCouponDTO couponDTO)
        {

            return new Coupon
            {
                Code = couponDTO.Code.Trim(),
                Name = couponDTO.Name.Trim(),
                Description = couponDTO.Description?.Trim(),
                DiscountType = couponDTO.DiscountType,
                DiscountValue = couponDTO.DiscountValue,
                MinOrderAmount = couponDTO.MinOrderAmount,
                MaxDiscountAmount = couponDTO.MaxDiscountAmount,
                MaxUses = couponDTO.MaxUses,
                MaxUsesPerUser = couponDTO.MaxUsesPerUser,
                UsedCount = 0,
                StartDate = couponDTO.StartDate,
                EndDate = couponDTO.EndDate,
                IsActive = couponDTO.IsActive,
                CreatedAt = DateTime.UtcNow
            };
        }
        private CouponResponseDTO MapCouponToResponseDto(Coupon coupon)
        {
            return new CouponResponseDTO
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
    }
}
