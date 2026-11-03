using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.CartDTOs;
using ShopService.Application.Interfaces;
using System.Security.Claims;

namespace ShopService.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        private Guid? GetCurrentUserId() =>
        Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
        [Authorize]
        [HttpPost]
        public async  Task<ActionResult<CartResponseDTO?>> CreateCart([FromBody] CreateCartDTO createCartDTO)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpDelete("{cartId:guid}")]
        public async Task<ActionResult> DeleteCart([FromRoute] Guid cartId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("items/{itemId:guid}")]
        public async Task<ActionResult<CartItemResponseDTO?>> GetCartItemById([FromRoute] Guid itemId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpPut("items/{itemId:guid}")]
        public async Task<ActionResult<CartItemResponseDTO?>> UpdateCartItem([FromBody] UpdateCartItemDTO updatedCartItemDTO,[FromRoute] Guid cartItemId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpDelete("items/{itemId:guid}")]
        public async Task<ActionResult> RemoveCartItemFromCart([FromRoute] Guid itemId)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpPost("items")]                  
        public async Task<ActionResult<CartItemResponseDTO>> AddCartItem([FromBody] AddCartItemDTO addCartItemDTO)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<CartItemResponseDTO>>> GetAllCartItems([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CartResponseDTO?>> GetMyCart()
        {
            throw new NotImplementedException();
        }
    }
}
