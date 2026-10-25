using Microsoft.EntityFrameworkCore;
using ShopService.Application.DTOs.CartDTOs;
using ShopService.Application.DTOs.CategoryDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;
        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<CartItem?> AddCartItemToCart(CartItem cartItem, Guid userId)
        {
            var cartExists = await _appDbContext.Carts
        .AnyAsync(c => c.Id == cartItem.CartId && c.UserId == userId);
            if (!cartExists) return null;
            var existing = await _appDbContext.CartItems
        .Include(e => e.Product)
        .FirstOrDefaultAsync(e => e.CartId == cartItem.CartId
                               && e.ProductId == cartItem.ProductId);
            if (existing is not null)
            {
                var totalRequested = existing.Quantity + cartItem.Quantity;

                if (existing.Product.StockQuantity < totalRequested)
                    return null;

                existing.Quantity = totalRequested;
                await _appDbContext.SaveChangesAsync();
                return existing;
            }
            var product = await _appDbContext.Products.FindAsync(cartItem.ProductId);
            if (product is null || product.StockQuantity < cartItem.Quantity)
                return null;
            cartItem.Product = product;
            cartItem.UnitPrice = product.Price;
            _appDbContext.CartItems.Add(cartItem);
            await _appDbContext.SaveChangesAsync();
            return cartItem;

        }

        public async Task<Cart?> CreateCart(Cart cart,Guid userId)
        {
            if (await _appDbContext.Carts.AnyAsync(e => e.UserId == userId))
                return null;

            cart.UserId = userId;
            _appDbContext.Carts.Add(cart);
            await _appDbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteCart(Guid cartId,Guid userId)
        {
            var cart = await _appDbContext.Carts
          .FirstOrDefaultAsync(e => e.Id == cartId && e.UserId == userId);
            if (cart is null) return false;
            _appDbContext.Carts.Remove(cart);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CartItemResponseDTO>> GetAllCartItems(Guid userId,int pageNumber = 1, int pageSize = 10)
        {
         return await _appDbContext.CartItems
        .Include(e=>e.Product)
        .Where(e => e.Cart.UserId == userId)
        .OrderBy(e => e.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)     
        .Select(MapToCartItemResponseDTO())
        .ToListAsync();

        }

        public async Task<CartItem?> GetCartItemById(Guid cartItemId)
        {
            var cartItem = await 
                _appDbContext.CartItems.Include(e=>e.Cart).Include(e=>e.Product).FirstOrDefaultAsync(e => e.Id == cartItemId);
            return cartItem;
        }

        public async Task<Cart?> GetUsersCart(Guid userId)
        {
            var cart = await 
                _appDbContext.Carts.Include(e=>e.Items).ThenInclude(e=>e.Product).FirstOrDefaultAsync(e =>e.UserId==userId);
            return cart;
        }

        public async Task<bool> RemoveCartItemFromCart(Guid cartItemId,Guid userId)
        {
            var result = await _appDbContext.CartItems.Include(e=>e.Cart).FirstOrDefaultAsync(e=>e.Id==cartItemId && e.Cart.UserId==userId);
            if (result is null) return false ;
             _appDbContext.Remove(result);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem?> UpdateCartItem(CartItem updatedCartItem, Guid cartItemId,Guid userId)
        {
            var cartItem = await _appDbContext.CartItems
            .Include(e => e.Product)
            .FirstOrDefaultAsync(e => e.Id == cartItemId && e.Cart.UserId == userId);
            if (cartItem is null || cartItem.Product is null) return null;
            if (updatedCartItem.Quantity <= 0 ||
                updatedCartItem.Quantity > cartItem.Product.StockQuantity)
                return null;
            cartItem.Quantity = updatedCartItem.Quantity;
            await _appDbContext.SaveChangesAsync();
            return cartItem;
        }
        private static Expression<Func<CartItem, CartItemResponseDTO>> MapToCartItemResponseDTO()
        {
            return i => new CartItemResponseDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product != null ? i.Product.Name : string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                CreatedAt=i.CreatedAt,
                UpdatedAt=i.UpdatedAt

            };
        }
    }
}
