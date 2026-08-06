using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.ProductDTOs
{
    public class ProductReviewResponseDTO
    {
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public Guid ReviewId { get; set; }
         public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewCreatedAt { get; set; }

    }
}
