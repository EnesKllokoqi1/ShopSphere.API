using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.CartDTOs
{
    public class CreateCartDTO
    {
        public Guid? UserId { get; set; }

        [MinLength(1, ErrorMessage = "Items list cannot be null.")]     
        public List<AddCartItemDTO> Items { get; set; } = new List<AddCartItemDTO>();
    }
}
