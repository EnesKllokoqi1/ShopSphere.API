using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class ShipOrderDTO
    {
        [Required]
        public string TrackingNumber { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
