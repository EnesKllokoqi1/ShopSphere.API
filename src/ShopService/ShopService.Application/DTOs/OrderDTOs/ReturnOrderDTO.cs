using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class ReturnOrderDTO
    {
        [Required(ErrorMessage = "Return reason is required")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Return reason must be between 3 and 500 characters")]
        public string Reason { get; set; } = string.Empty;
    }
}
