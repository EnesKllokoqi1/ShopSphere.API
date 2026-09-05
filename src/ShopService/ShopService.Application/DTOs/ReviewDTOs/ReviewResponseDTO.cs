using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.ReviewDTOs
{
    public class ReviewResponseDTO
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
