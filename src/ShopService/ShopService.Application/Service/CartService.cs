using ShopService.Application.DTOs.CartDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public Task<CartItemResponseDTO?> AddCartItemToCart(AddCartItemDTO cartItem, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartResponseDTO?> CreateCart(CreateCartDTO createCartDTO, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCart(Guid cartId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CartItemResponseDTO>> GetAllCartItems(Guid userId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemResponseDTO?> GetCartItemById(Guid cartItemId)
        {
            throw new NotImplementedException();
        }

        public Task<CartResponseDTO?> GetUsersCart(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCartItemFromCart(Guid CartItemId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemResponseDTO?> UpdateCartItem(CartItem updatedCartItem, Guid cartItemId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
