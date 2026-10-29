using ShopService.Application.DTOs.CartDTOs;
using ShopService.Application.DTOs.UserDTOs;
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
        public async Task<CartItemResponseDTO?> AddCartItemToCart(AddCartItemDTO cartItemDTO, Guid userId)
        {
            var cartItem = MapAddCartItemDTO(cartItemDTO);
            var result = await _cartRepository.AddCartItemToCart(cartItem, userId);
            if (result is null)
            {
                return null;
            }
            return MapCartItemResponseDto(result);
        }

        private CartItemResponseDTO MapCartItemResponseDto(CartItem cartItem)
        {
            return new CartItemResponseDTO
            {
                Id = cartItem.Id,
                ProductId=cartItem.ProductId,
                ProductName=cartItem.Product.Name,
                Quantity=cartItem.Quantity,
                UnitPrice=cartItem.UnitPrice,
                CreatedAt=cartItem.CreatedAt,
                UpdatedAt=cartItem.UpdatedAt,
            };
        }
        private Cart MapCreateCartDTO(CreateCartDTO createCartDTO,Guid userId)
        {
            var now = DateTime.UtcNow;
            return new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = now,
                UpdatedAt = now,
                Items = createCartDTO.Items
                    .Select(i => new CartItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };
        }
        public async Task<CartResponseDTO?> CreateCartAsync(CreateCartDTO createCartDTO, Guid userId)
        {
            var cart = MapCreateCartDTO(createCartDTO, userId);
            var result = await _cartRepository.CreateCart(cart,userId);
            if (result is null)
            {
                return null;
            }
            return MapCartResponseDTO(result);
        }

        private CartResponseDTO? MapCartResponseDTO(Cart cart)
        {
            return new CartResponseDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items.Select(MapCartItemResponseDto).ToList()
            };
        }

        public async Task<bool> DeleteCart(Guid cartId, Guid userId)
        {
            return await _cartRepository.DeleteCart(cartId, userId);
        }

        public async Task<IEnumerable<CartItemResponseDTO>> GetAllCartItems(Guid userId, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            return await _cartRepository.GetAllCartItems(userId, pageNumber, pageSize);
        }

        public async Task<CartItemResponseDTO?> GetCartItemByIdAsync(Guid cartItemId,Guid userId)
        {
            var cartItem = await _cartRepository.GetCartItemById(cartItemId,userId);
            if (cartItem is null)
            {
                return null;
            }
            return MapCartItemResponseDto(cartItem);
        }

        public async Task<CartResponseDTO?> GetUsersCart(Guid userId)
        {
            var usersCart = await _cartRepository.GetUsersCart(userId);
            if (usersCart is null)
            {
                return null;
            }
            return MapCartResponseDTO(usersCart);
        }

        public async Task<bool> RemoveCartItemFromCart(Guid CartItemId, Guid userId)
        {
            return await _cartRepository.RemoveCartItemFromCart(CartItemId, userId);
        }

        public async Task<CartItemResponseDTO?> UpdateCartItem(UpdateCartItemDTO updatedCartItemDTO, Guid cartItemId, Guid userId)
        {
            var updatedCartItem = MapUpdateCartItemDTO(updatedCartItemDTO);
            var cartItem = await _cartRepository.UpdateCartItem(updatedCartItem, cartItemId, userId);
            if (cartItem is null)
            {
                return null;
            }
            return MapCartItemResponseDto(cartItem);
        }
        private CartItem MapUpdateCartItemDTO(UpdateCartItemDTO dto)
        {
            return new CartItem
            {
                Quantity = dto.Quantity
            };
        }
        public CartItem MapAddCartItemDTO(AddCartItemDTO addCartItemDTO)
        {
            return new CartItem
            {
                ProductId = addCartItemDTO.ProductId,
                Quantity = addCartItemDTO.Quantity
            };
        }
    }
}
