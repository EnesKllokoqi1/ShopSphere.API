using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository,ICouponRepository couponRepository)
        {
            _orderRepository = orderRepository;
            _couponRepository = couponRepository;
        }
        public async Task<OrderResponseDTO?> CancelOrderAsync(Guid orderId, string? reason = null)
        {
            var result = await _orderRepository.CancelOrderAsync(orderId, reason);
            if (result is null)
            {
                return null;
            }
            var cancelledOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(cancelledOrder);
        }

        public async Task<OrderResponseDTO?> ConfirmOrderAsync(Guid orderId)
        {
            var result = await _orderRepository.ConfirmOrderAsync(orderId);
            if (result is null)
            {
                return null;
            }
            var confirmedOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(confirmedOrder);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task<OrderResponseDTO?> DeliverOrderAsync(Guid orderId, OrderNotesDTO? deliveryNotes = null)
        {
            var result = await _orderRepository.DeliverOrderAsync(orderId,deliveryNotes?.Notes);
            if (result is null)
            {
                return null;
            }
            var deliveredOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(deliveredOrder);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var orders = await _orderRepository.GetAllAsync(pageNumber, pageSize);
            return orders;
        }

        public async Task<OrderResponseDTO?> GetOrderByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.GetOrderByIdAsync(orderId);
            if (result is null)
            {
                return null; 
            }
          return  MapToOrderResponseDto(result);
        }

        public async Task<int> GetOrderCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orderCount = await _orderRepository.GetOrderCountByDateRangeAsync(startDate,endDate);
            return orderCount;
        }

        public async Task<int> GetOrderCountByStatusAsync(OrderStatus orderStatus)
        {
            var orderCount = await _orderRepository.GetOrderCountByStatusAsync(orderStatus);
            return orderCount;
        }

        public async Task<int> GetOrderCountByUserAsync(Guid userId)
        {
            var orderCount = await _orderRepository.GetOrderCountByUserAsync(userId);
            return orderCount;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var prdersByDateRangeAsync = await _orderRepository.GetOrdersByDateRangeAsync(startDate,endDate);
            return prdersByDateRangeAsync;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByStatusAsync(OrderStatus orderStatus, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var ordersByStatus = await _orderRepository.GetOrdersByStatusAsync(orderStatus, pageNumber, pageSize);
            return ordersByStatus;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var ordersByUserId = await _orderRepository.GetOrdersByUserIdAsync(userId, pageNumber, pageSize);
            return ordersByUserId;
        }

        public async Task<OrderStatus?> GetOrderStatusAsync(Guid orderId)
        {
            var orderStatus = await _orderRepository.GetOrderStatusAsync(orderId);
            if (orderStatus is null)
            {
                return null;
            }
            return orderStatus;
        }

        public async Task<int> GetTotalOrderCountAsync()
        {
            return await _orderRepository.GetTotalOrderCountAsync();
        }

        public async Task<decimal?> GetTotalSpentByUserAsync(Guid userId)
        {
            var result = await _orderRepository.GetTotalSpentByUserAsync(userId);
            if (result is null)
            {
                return null;
            }
            return result;
        }

        public async Task<OrderResponseDTO?> MakeOrderAsync(PlaceOrderDTO placeOrderDTO,Guid userId)
        {
            var order = MapToOrder(placeOrderDTO, userId);
            var subtotal = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            if (!string.IsNullOrWhiteSpace(placeOrderDTO.CouponCode))
            {
                var (success, error) = await ApplyCouponAsync(order, placeOrderDTO.CouponCode, subtotal);
                if (!success)
                {
                    throw new InvalidOperationException(error);
                }
            }
            else
            {
                order.TotalAmount = subtotal;
            }
            int attempt = 0;
            var result = await _orderRepository.MakeOrderAsync(order,attempt);
            var createdOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            if (createdOrder is null)
            {
                throw new InvalidOperationException("Order was created but could not be retrieved.");
            }
            return MapToOrderResponseDto(createdOrder);
        }

        public async Task<OrderResponseDTO?> ProcessOrderAsync(Guid orderId, OrderNotesDTO? notes = null)
        {
            var result = await _orderRepository.ProcessOrderAsync(orderId, notes?.Notes);
            if (result is null)
            {
                return null;
            }
            var processedOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(processedOrder);
        }

        public async Task<OrderResponseDTO?> ReturnOrderAsync(Guid orderId, ReturnOrderDTO returnOrderDTO)
        {
            var result = await _orderRepository.ReturnOrderAsync(orderId, returnOrderDTO.Reason);
            if (result is null)
            {
                return null;
            }
            var returnedOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(returnedOrder);
        }

        public async Task<OrderResponseDTO?> ShipOrderAsync(Guid orderId, ShipOrderDTO shipOrderDTO)
        {
            var result = await _orderRepository.ShipOrderAsync(orderId,shipOrderDTO.TrackingNumber,shipOrderDTO.Notes);
            if (result is null)
            {
                return null;
            }
            var shippedOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(shippedOrder);
        }

        public async Task<OrderResponseDTO?> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO, Guid orderId)
        {
            var order = new Order
            {
                ShippingMethod = updateOrderDTO.ShippingMethod ?? ShippingMethod.Standard,
                ShippingAddress = updateOrderDTO.ShippingAddress,
                BillingAddress = updateOrderDTO.BillingAddress,
                TrackingNumber = updateOrderDTO.TrackingNumber,
                Notes = updateOrderDTO.Notes,
                UpdatedAt = DateTime.UtcNow
            };
            var result = await _orderRepository.UpdateOrderAsync(order, orderId);
            if (result is null)
            {
                return null;
            }
            var updatedOrder = await _orderRepository.GetOrderByIdAsync(result.Id);
            return MapToOrderResponseDto(updatedOrder);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid orderId, UpdatePaymentStatusDTO paymentStatus)
        {
            var result = await _orderRepository.UpdatePaymentStatusAsync(orderId, paymentStatus.Status);
            if (!result)
            {
                return false;
            }
            return true;
        }
        private Order MapToOrder(PlaceOrderDTO placeOrderDTO, Guid userId)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ShippingAddress = placeOrderDTO.ShippingAddress,
                BillingAddress = placeOrderDTO.BillingAddress,
                PaymentMethod = placeOrderDTO.PaymentMethod,
                PaymentIntentId = placeOrderDTO.PaymentIntentId,
                ShippingMethod = placeOrderDTO.ShippingMethod,
                Notes = placeOrderDTO.Notes,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = placeOrderDTO.OrderItems.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            return order;
        }
        private OrderResponseDTO MapToOrderResponseDto(Order order)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                UserName = order.User.FirstName,
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
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? string.Empty,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Quantity * item.UnitPrice
                }).ToList()
            };
        }
        private async Task<(bool Success, string? Error)> ApplyCouponAsync(Order order, string couponCode, decimal subtotal)
        {
            var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode.Trim().ToUpperInvariant());

            if (coupon is null || !coupon.IsActive)
                return (false, "Invalid coupon.");

            var now = DateTime.UtcNow;
            if (now < coupon.StartDate || (coupon.EndDate.HasValue && now > coupon.EndDate.Value))
                return (false, "Coupon is not valid at this time.");

            if (coupon.MinOrderAmount.HasValue && subtotal < coupon.MinOrderAmount.Value)
                return (false, "Order total is too low for this coupon.");

            if (coupon.MaxUsesPerUser.HasValue && order.UserId.HasValue)
            {
                var used = await _couponRepository.GetUserUsageCountAsync(coupon.Id, order.UserId.Value);
                if (used >= coupon.MaxUsesPerUser.Value)
                    return (false, "You have already used this coupon the maximum number of times.");
            }
            if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses.Value)
                return (false, "Coupon usage limit reached.");

            var discount = coupon.DiscountType == DiscountType.Percentage
                ? subtotal * coupon.DiscountValue / 100
                : coupon.DiscountValue;

            if (coupon.MaxDiscountAmount.HasValue)
                discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

            order.CouponId = coupon.Id;
            order.TotalAmount = Math.Max(0, subtotal - discount);
            return (true, null);
        }
    }
}
