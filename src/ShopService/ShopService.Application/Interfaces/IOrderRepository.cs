using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Domain.Entities;
using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> MakeOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IReadOnlyList<OrderResponseDTO>> GetAllAsync(int pageNumber,int pageSize);
        Task<bool> DeleteOrderAsync(Guid orderId);
        Task<Order?> UpdateOrderAsync(Order updatedOrder, Guid orderId);
        Task<OrderStatus> GetOrderStatusAsync(Guid orderId);
        Task<Order?> CancelOrderAsync(Guid orderId, string? reason = null);
        Task<Order?> ConfirmOrderAsync(Guid orderId);
        Task<Order?> ProcessOrderAsync(Guid orderId, string? notes = null);
        Task<Order?> ShipOrderAsync(Guid orderId, string trackingNumber, string? notes = null);
        Task<Order?> DeliverOrderAsync(Guid orderId);    
        Task<Order?> ReturnOrderAsync(Guid orderId, string reason);
        Task<bool> UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status);
        Task<IReadOnlyList<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId,int pageNumber,int pageSize);
        Task<IReadOnlyList<OrderResponseDTO>> GetOrdersByStatusAsync(OrderStatus status,int pageNumber,int pageSize);
        Task<decimal> GetTotalSpentByUserAsync(Guid userId);
        Task<IReadOnlyList<OrderResponseDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> OrderExistsAsync(Guid orderId);
        Task<int> GetOrderCountByUserAsync(Guid userId);
        Task<int> GetTotalOrderCountAsync();
        Task<int> GetOrderCountByStatusAsync(OrderStatus status);
        Task<int> GetOrderCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    }       
}
