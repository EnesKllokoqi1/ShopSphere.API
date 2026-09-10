using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class PlaceOrderDTO
    {
        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Shipping address must be between 10 and 500 characters")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing address is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Billing address must be between 10 and 500 characters")]
        public string BillingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Payment method must be between 3 and 50 characters")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment intent ID is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Payment intent ID must be between 5 and 100 characters")]
        public string PaymentIntentId { get; set; } = string.Empty;

        public ShippingMethod ShippingMethod { get; set; } = ShippingMethod.Standard;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "At least one item is required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        [MaxLength(50, ErrorMessage = "Maximum 50 items per order")]
        public IEnumerable<PlaceOrderItemDTO> OrderItems { get; set; } = new List<PlaceOrderItemDTO>();
    }

    public class PlaceOrderItemDTO
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 999, ErrorMessage = "Quantity must be between 1 and 999")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0.01, 99999.99, ErrorMessage = "Unit price must be between $0.01 and $99,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Unit price must have up to 2 decimal places")]
        public decimal UnitPrice { get; set; }
    }
}