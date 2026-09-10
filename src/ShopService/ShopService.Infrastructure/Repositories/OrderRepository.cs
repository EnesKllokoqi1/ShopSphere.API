using Microsoft.EntityFrameworkCore;
using Npgsql;
using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Application.DTOs.ReviewDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Enums;
using ShopService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Order?> CancelOrderAsync(Guid orderId, string? reason = null)
        {
            await using var transaction = await _appDbContext.Database
           .BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var order = await GetOrderByIdAsync(orderId);

                if (order is null)
                {
                    return null;
                }

                if (order.OrderStatus != OrderStatus.Pending &&
                    order.OrderStatus != OrderStatus.Confirmed)
                {
                    return null;
                }

                order.Notes = !string.IsNullOrEmpty(reason)
                    ? $"Cancelled: {reason}"
                    : "Cancelled by user";

                var productIds = order.OrderItems
                    .Where(x => x.ProductId.HasValue)
                    .Select(x => x.ProductId!.Value)
                    .Distinct()
                    .ToList();

                var products = await _appDbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id);

                foreach (var item in order.OrderItems)
                {
                    if (!item.ProductId.HasValue ||
                        !products.TryGetValue(item.ProductId.Value, out var product))
                    {
                        return null;
                    }

                    product.StockQuantity += item.Quantity;
                }

                order.OrderStatus = OrderStatus.Cancelled;

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order?> ConfirmOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null) return null; 
            if (order.OrderStatus!=OrderStatus.Pending)
            {
                return null;
            }
            order.OrderStatus = OrderStatus.Confirmed;
            await _appDbContext.SaveChangesAsync();
            return order;
          
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return false;
            }
            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Order?> DeliverOrderAsync(Guid orderId, string? deliveryNotes = null)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null) return null;
            if (order.OrderStatus != OrderStatus.Shipped) return null;
            if (!string.IsNullOrEmpty(deliveryNotes))
            {
                order.Notes = deliveryNotes;
            }
            order.OrderStatus = OrderStatus.Delivered;
            await _appDbContext.SaveChangesAsync();
            return order;
           
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _appDbContext.Orders
                .AsNoTracking()
                  .Include(o => o.User) 
                  .Include(o => o.OrderItems)  
                  .ThenInclude(i => i.Product)
                  .OrderBy(e => e.CreatedAt)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .Select(MapToOrderResponseDTO())
                  .ToListAsync();
        }
        private static Expression<Func<Order, OrderResponseDTO>> MapToOrderResponseDTO()
        {
            return order => new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                UserName = order.User != null ? order.User.FirstName : null,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                ShippingMethod = order.ShippingMethod,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                PaymentIntentId = order.PaymentIntentId,
                TrackingNumber = order.TrackingNumber,
                Notes = order.Notes,
                OrderItems = order.OrderItems
           .Select(item => new OrderItemResponseDTO
           {
               Id = item.Id,
               ProductId = item.ProductId,
               ProductName = item.Product != null ? item.Product.Name : string.Empty,
               Quantity = item.Quantity,
               UnitPrice = item.UnitPrice,
               Subtotal = item.Quantity * item.UnitPrice
           })
            };

        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _appDbContext.Orders
                   .Include(o => o.User)
                   .Include(o => o.OrderItems)
                   .ThenInclude(i => i.Product).FirstOrDefaultAsync(o=>o.Id==orderId);
        }

        public async Task<int> GetOrderCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var count = await _appDbContext.Orders
            .CountAsync(o => o.CreatedAt >= startDate && o.CreatedAt < endDate);
            return count;
        }

        public async Task<int> GetOrderCountByStatusAsync(OrderStatus status)
        {
            var count = await _appDbContext.Orders
             .CountAsync(o =>o.OrderStatus==status);
            return count;
        }

        public async Task<int> GetOrderCountByUserAsync(Guid userId)
        {
            var count = await _appDbContext.Orders
              .CountAsync(o => o.UserId == userId);
            return count;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _appDbContext.Orders
                 .AsNoTracking()
                 .Include(o => o.User)
                 .Include(o => o.OrderItems)
                 .ThenInclude(p => p.Product)
                 .Where(e => e.CreatedAt >= startDate && e.CreatedAt < endDate)
                 .Select(MapToOrderResponseDTO())
                 .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByStatusAsync(OrderStatus status, int pageNumber, int pageSize)
        {
            var orders = await _appDbContext.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .Where(o => o.OrderStatus == status)
                .OrderBy(o => o.CreatedAt)
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .Select(MapToOrderResponseDTO())
                .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var orders = await _appDbContext.Orders
               .AsNoTracking()
               .Include(o => o.User)
               .Include(o => o.OrderItems)
               .ThenInclude(p => p.Product)
               .Where(e => e.UserId == userId)
               .OrderBy(o => o.CreatedAt)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(MapToOrderResponseDTO())
               .ToListAsync();
            return orders;
        }

        public async Task<OrderStatus?> GetOrderStatusAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return null;
            }
            return order.OrderStatus;
        }

        public async Task<int> GetTotalOrderCountAsync()
        {
            return await _appDbContext.Orders.CountAsync();
        }

        public async Task<decimal?> GetTotalSpentByUserAsync(Guid userId)
        {
            var total = await _appDbContext.Orders
                       .Where(o => o.UserId == userId)
                       .SumAsync(o => (decimal?)o.TotalAmount);
                        return total;
        }

        public async Task<Order?> MakeOrderAsync(Order order,int attempt=1)
        {
            if (attempt > 5)
            {
                throw new InvalidOperationException("Could not generate a unique order number after several tries.");
            }

            if (order.OrderItems.Any(x => !x.ProductId.HasValue))
            {
                return null;
            }

            var productIds = order.OrderItems
                .Select(x => x.ProductId!.Value)
                .Distinct()
                .ToList();

            await using var transaction = await _appDbContext.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var products = await _appDbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id);
                foreach (var item in order.OrderItems)
                {
                    if (!products.TryGetValue(item.ProductId!.Value, out var product))
                    {
                        return null;
                    }
                    if (product.StockQuantity < item.Quantity)
                    {
                        return null;
                    }
                }
                foreach (var item in order.OrderItems)
                {
                    products[item.ProductId!.Value].StockQuantity -= item.Quantity;
                }

                order.OrderNumber = await GenerateOrderNumberAsync();
                _appDbContext.Orders.Add(order);

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex) || IsSerializationFailure(ex))
            {
                await transaction.RollbackAsync();
                _appDbContext.Entry(order).State = EntityState.Detached;
                return await MakeOrderAsync(order, attempt + 1);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private static bool IsSerializationFailure(DbUpdateException ex)
        {
            return ex.InnerException is PostgresException pgEx && pgEx.SqlState == "40001";
        }
        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505";
        }
        public async Task<string> GenerateOrderNumberAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var prefix = $"ORD-{today}";

            var count = await _appDbContext.Orders
                .CountAsync(o => o.OrderNumber.StartsWith(prefix));

            var sequence = count + 1;
            return $"{prefix}-{sequence:D4}";
        }

        public async Task<Order?> ProcessOrderAsync(Guid orderId, string? notes = null)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return null;
            }

            if (order.OrderStatus != OrderStatus.Confirmed)
            {
                return null;
            }

            order.OrderStatus = OrderStatus.Processing;

            if (!string.IsNullOrWhiteSpace(notes))
            {
                order.Notes = notes;
            }
            await _appDbContext.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> ReturnOrderAsync(Guid orderId, string reason)
        {
            await using var transaction = await _appDbContext.Database
         .BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var order = await GetOrderByIdAsync(orderId);
                if (order is null)
                {
                    return null;
                }
                if (order.OrderStatus != OrderStatus.Delivered)
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(reason))
                {
                    return null;
                }

                var productIds = order.OrderItems
                    .Where(x => x.ProductId.HasValue)
                    .Select(x => x.ProductId!.Value)
                    .Distinct()
                    .ToList();

                var products = await _appDbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id);

                foreach (var item in order.OrderItems)
                {
                    if (!item.ProductId.HasValue ||
                        !products.TryGetValue(item.ProductId.Value, out var product))
                    {
                        return null;
                    }

                    product.StockQuantity += item.Quantity;
                }
                order.OrderStatus = OrderStatus.Returned;
                order.Notes = $"Returned: {reason}";
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order?> ShipOrderAsync(Guid orderId, string trackingNumber, string? notes = null)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return null;
            }
            if (order.OrderStatus != OrderStatus.Processing) return null;
            if (!string.IsNullOrEmpty(notes))
            {
                order.Notes = notes;    
            }
            order.TrackingNumber = trackingNumber;
            order.OrderStatus = OrderStatus.Shipped;
            await _appDbContext.SaveChangesAsync();
            return order;

        }

        public async Task<Order?> UpdateOrderAsync(Order updatedOrder, Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return null;
            }
            order.ShippingAddress = updatedOrder.ShippingAddress;
            order.ShippingMethod = updatedOrder.ShippingMethod;
            order.BillingAddress = updatedOrder.BillingAddress;
            order.Notes = updatedOrder.Notes;
            await _appDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return false;
            }
            order.PaymentStatus = status;
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
