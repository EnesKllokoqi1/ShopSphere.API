using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO?> MakeOrderAsync(PlaceOrderDTO placeOrderDTO);
        Task<OrderResponseDTO?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task<bool> DeleteOrderAsync(Guid orderId);
        Task<OrderResponseDTO?> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO, Guid orderId);
        Task<OrderStatus?> GetOrderStatusAsync(Guid orderId);
        Task<OrderResponseDTO?> CancelOrderAsync(Guid orderId, string? reason = null);
        Task<OrderResponseDTO?> ConfirmOrderAsync(Guid orderId);
        Task<OrderResponseDTO?> ProcessOrderAsync(Guid orderId, OrderNotesDTO? notes = null);
        Task<OrderResponseDTO?> ShipOrderAsync(Guid orderId, ShipOrderDTO shipOrderDTO);
        Task<OrderResponseDTO?> DeliverOrderAsync(Guid orderId, OrderNotesDTO? deliveryNotes = null);
        Task<OrderResponseDTO?> ReturnOrderAsync(Guid orderId, ReturnOrderDTO returnOrderDTO);
        Task<bool> UpdatePaymentStatusAsync(Guid orderId, UpdatePaymentStatusDTO paymentStatus);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByStatusAsync(OrderStatus orderStatus, int pageNumber, int pageSize);
        Task<decimal?> GetTotalSpentByUserAsync(Guid userId);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetOrderCountByUserAsync(Guid userId);
        Task<int> GetTotalOrderCountAsync();
        Task<int> GetOrderCountByStatusAsync(OrderStatus orderStatus);
        Task<int> GetOrderCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}