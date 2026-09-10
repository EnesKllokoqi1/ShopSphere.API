using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public IEnumerable<OrderItemResponseDTO> OrderItems { get; set; } = new List<OrderItemResponseDTO>();
        public bool CanBeCanceled => OrderStatus == OrderStatus.Pending || OrderStatus == OrderStatus.Confirmed;
    }

    public class OrderItemResponseDTO
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}