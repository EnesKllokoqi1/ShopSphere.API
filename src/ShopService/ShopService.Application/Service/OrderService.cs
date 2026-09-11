using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Application.Interfaces;
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
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Task<OrderResponseDTO?> CancelOrderAsync(Guid orderId, string? reason = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> ConfirmOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> DeliverOrderAsync(Guid orderId, OrderNotesDTO? deliveryNotes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOrderCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOrderCountByStatusAsync(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOrderCountByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponseDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponseDTO>> GetOrdersByStatusAsync(OrderStatus orderStatus, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<OrderStatus?> GetOrderStatusAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalOrderCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> GetTotalSpentByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> MakeOrderAsync(PlaceOrderDTO placeOrderDTO)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> ProcessOrderAsync(Guid orderId, OrderNotesDTO? notes = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> ReturnOrderAsync(Guid orderId, ReturnOrderDTO returnOrderDTO)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> ShipOrderAsync(Guid orderId, ShipOrderDTO shipOrderDTO)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponseDTO?> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO, Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePaymentStatusAsync(Guid orderId, UpdatePaymentStatusDTO paymentStatus)
        {
            throw new NotImplementedException();
        }
    }
}
