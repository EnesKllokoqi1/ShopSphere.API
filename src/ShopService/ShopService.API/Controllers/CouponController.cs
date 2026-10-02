using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.CouponDTOs;
using ShopService.Application.Interfaces;
using ShopService.Application.Service;
using ShopService.Domain.Enums;
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
        private bool IsOwnerOrAdmin(Guid? ownerId) =>
      User.IsInRole(nameof(UserRole.Admin)) ||
      (ownerId is not null && GetCurrentUserId() == ownerId);
        [Authorize(Policy ="AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<CouponResposneDTO>> CreateCoupon([FromBody]CreateCouponDTO createCouponDTO)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPut("{couponId:guid}")]
        public async Task<ActionResult<CouponResposneDTO>> UpdateCoupon([FromBody] UpdateCouponDTO updateCouponDTO, [FromRoute] Guid couponId)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{couponId:guid}")]
        public async Task<ActionResult<CouponResposneDTO>> DeleteCoupon([FromRoute] Guid couponId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("check-duplicate")]
        public async Task<ActionResult> CheckDuplicateCoupon([FromQuery] string code)
        {
            throw new NotImplementedException();    
        }
        [Authorize]
        [HttpGet("by-code")]
        public async Task<ActionResult<CouponResposneDTO>> GetCouponByCode([FromQuery] string code)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("{couponId:guid}")]
        public async Task<ActionResult<CouponResposneDTO>> GetCouponById([FromRoute] Guid couponId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponResposneDTO>>> GetAllCouponsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("by-product/{productId:guid}")]
        public async Task<ActionResult<IEnumerable<CouponResposneDTO>>> GetCouponsLinkedToProduct([FromRoute] Guid productId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("{couponId:guid}/products")]
        public async Task<ActionResult<IEnumerable<ProductSummaryDTO>>> GetProductsLinkedToCoupon([FromRoute] Guid couponId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPost("{couponId:guid}/products/{productId:guid}")]
        public async Task<ActionResult> AddProductToCouponAsync([FromRoute] Guid couponId, [FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpDelete("{couponId:guid}/products/{productId:guid}")]
        public async Task<ActionResult> RemoveProductFromCoupon([FromRoute] Guid couponId, [FromRoute] Guid productId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("{couponId:guid}/my-usage")]
        public async Task<ActionResult> GetUserUsageCount([FromRoute] Guid couponId)
        {
            throw new NotImplementedException();
        }
    }
}
