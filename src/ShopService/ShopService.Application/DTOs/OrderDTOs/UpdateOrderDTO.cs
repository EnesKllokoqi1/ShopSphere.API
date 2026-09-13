using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class UpdateOrderDTO
    {
        [EnumDataType(typeof(ShippingMethod))]
        public ShippingMethod? ShippingMethod { get; set; }
        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string BillingAddress { get; set; } = null!;

        [StringLength(50)]
        public string? TrackingNumber { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

    }
}
