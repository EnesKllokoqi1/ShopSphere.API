using ShopService.Application.DTOs.CartDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> CreateCart(Cart cart, Guid userId);
        Task<bool> RemoveCartItemFromCart(Guid cartItemId, Guid userId);
        Task<CartItem?> AddCartItemToCart(CartItem cartItem,Guid userId);
        Task<CartItem?> UpdateCartItem(CartItem updatedCartItem, Guid cartItemId, Guid userId);
        Task<bool> DeleteCart(Guid cartId,Guid userId);
        Task<IEnumerable<CartItemResponseDTO>> GetAllCartItems(Guid userId,int pageNumber,int pageSize);
        Task<CartItem?> GetCartItemById(Guid cartItemId);
        Task<Cart?> GetUsersCart(Guid userId);

    }
}
