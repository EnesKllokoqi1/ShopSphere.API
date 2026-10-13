using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.CartDTOs
{
    public class CartResponseDTO
    {

        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<CartItemResponseDTO> Items { get; set; } = new List<CartItemResponseDTO>();
        public decimal TotalPrice => Items.Sum(i => i.UnitPrice * i.Quantity);
        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}
