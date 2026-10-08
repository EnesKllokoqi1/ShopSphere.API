using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Application.Interfaces;
using ShopService.Application.Service;
using ShopService.Domain.Entities;
using ShopService.Domain.Enums;
using Sprache;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ShopService.API.Controllers
{
    [Route("api/coupons")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        private Guid? GetCurrentUserId() =>
          Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
        [Authorize(Policy ="AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<CouponResponseDTO>> CreateCoupon([FromBody]CreateCouponDTO createCouponDTO)
        {
            var coupon = await _couponService.CreateCouponAsync(createCouponDTO);
            if (coupon is null)
            {
                return Conflict(new
                {
                    Status = "Error",
                    Message = "A coupon with that code already exists",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return CreatedAtAction(
                     nameof(GetCouponById),
                     new { couponId = coupon.Id },
                     coupon
                 ); ;
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPut("{couponId:guid}")]
        public async Task<ActionResult<CouponResponseDTO>> UpdateCoupon([FromBody] UpdateCouponDTO updateCouponDTO, [FromRoute] Guid couponId)
        {
            var coupon = await _couponService.UpdateCouponAsync(updateCouponDTO,couponId);
            if (coupon is null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Coupon not found or coupon code already exists",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Coupon has been successfuly updated",
                Coupon = coupon,
                TimeStamp = DateTime.UtcNow
            });
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{couponId:guid}")]
        public async Task<ActionResult<CouponResponseDTO>> DeleteCoupon([FromRoute] Guid couponId)
        {
            var result = await _couponService.DeleteCouponAsync(couponId);
            if (!result)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Coupon has not been found",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("check-duplicate")]
        public async Task<ActionResult> CheckDuplicateCoupon([FromQuery] string code)
        {
            var result = await _couponService.CodeExistsAsync(code);
            if (result)
            {
                return Conflict(new
                {
                    Status = "Error",
                    Message = "Coupons exist with that code",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Code is available",
                TimeStamp = DateTime.UtcNow
            });
        }
        [AllowAnonymous]
        [HttpGet("by-code")]
        public async Task<ActionResult<CouponResponseDTO>> GetCouponByCode([FromQuery] string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon is null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Coupon not found",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Coupon has been found",
                Coupon = coupon,
                TimeStamp = DateTime.UtcNow
            });
        }
        [AllowAnonymous]
        [HttpGet("{couponId:guid}")]
        public async Task<ActionResult<CouponResponseDTO>> GetCouponById([FromRoute] Guid couponId)
        {
            var coupon = await _couponService.GetCouponByIdAsync(couponId);
            if (coupon is null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message= "Coupon not found",
                    TimeStamp =DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Coupon has been found",
                Coupon = coupon,
                TimeStamp = DateTime.UtcNow
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponResponseDTO>>> GetAllCouponsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var coupons = await _couponService.GetAllCouponsAsync(pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {coupons.Count()} coupons.",
                Data = coupons,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }
        [AllowAnonymous]
        [HttpGet("by-product/{productId:guid}")]
        public async Task<ActionResult<IEnumerable<CouponResponseDTO>>> GetCouponsLinkedToProduct([FromRoute] Guid productId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
           var couponsLinkedToProduct=  await _couponService.GetCouponsLinkedToProduct(productId, pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {couponsLinkedToProduct.Count()} coupons.",
                Data = couponsLinkedToProduct,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });

        }
        [AllowAnonymous]
        [HttpGet("{couponId:guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDTO>>> GetProductsLinkedToCoupon([FromRoute] Guid couponId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var productsLinkedToCoupon = await _couponService.GetProductsLinkedToCoupon(couponId, pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {productsLinkedToCoupon.Count()} products.",
                Data = productsLinkedToCoupon,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPost("{couponId:guid}/products/{productId:guid}")]
        public async Task<ActionResult> AddProductToCouponAsync([FromRoute] Guid couponId, [FromRoute] Guid productId)
        {
            var result = await _couponService.AddProductToCouponAsync(couponId, productId);
            if (!result)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Coupon or product has not been found",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Product has been added to coupon",
                TimeStamp = DateTime.UtcNow
            });
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpDelete("{couponId:guid}/products/{productId:guid}")]
        public async Task<ActionResult> RemoveProductFromCoupon([FromRoute] Guid couponId, [FromRoute] Guid productId)
        {
            var result = await _couponService.RemoveProductFromCouponAsync(couponId,productId);
            if (!result)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Coupon or product has not been found",
                    TimeStamp = DateTime.UtcNow
                });
            }
            return NoContent(); 
        }
        [Authorize]
        [HttpGet("{couponId:guid}/my-usage")]
        public async Task<ActionResult> GetUserUsageCount([FromRoute] Guid couponId)
        {
            var userId =  GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized(new
                {
                    Status = "Error",
                    Message = "User has not been authenticated",
                    TimeStamp = DateTime.UtcNow
                });
            }
            var count = await _couponService.GetUserUsageCountAsync(couponId, userId.Value);
            return Ok(new
            {
                Status = "Success",
                UserUsageCount = count,
                TimeStamp = DateTime.UtcNow
            });
        }
    }
}
