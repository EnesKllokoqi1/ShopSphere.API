using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.OrderDTOs
{
    public class UpdatePaymentStatusDTO
    {
        [Required(ErrorMessage = "PaymentStatus is required")]
        public PaymentStatus Status { get; set; }
    }
}
