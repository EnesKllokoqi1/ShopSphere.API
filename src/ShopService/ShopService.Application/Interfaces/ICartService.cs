using ShopService.Application.DTOs.CartDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICartService
    {       
        Task<CartResponseDTO?> CreateCartAsync(CreateCartDTO createCartDTO, Guid userId);
        Task<bool> RemoveCartItemFromCart(Guid CartItemId, Guid userId);
        Task<CartItemResponseDTO?> AddCartItemToCart(AddCartItemDTO cartItem, Guid userId);
        Task<bool> DeleteCart(Guid cartId, Guid userId);
        Task<CartItemResponseDTO?> UpdateCartItem(UpdateCartItemDTO updatedCartItemDTO, Guid cartItemId, Guid userId);
        Task<IEnumerable<CartItemResponseDTO>> GetAllCartItems(Guid userId, int pageNumber, int pageSize);
        Task<CartItemResponseDTO?> GetCartItemByIdAsync(Guid cartItemId,Guid userId);
        Task<CartResponseDTO?> GetUsersCart(Guid userId);
    }
}
