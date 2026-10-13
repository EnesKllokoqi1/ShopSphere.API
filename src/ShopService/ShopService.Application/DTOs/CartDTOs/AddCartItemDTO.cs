using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.CartDTOs
{
    public class AddCartItemDTO
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int Quantity { get; set; }
    }
}
