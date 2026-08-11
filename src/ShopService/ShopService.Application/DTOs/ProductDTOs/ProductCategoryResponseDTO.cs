using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.ProductDTOs
{
    public class ProductCategoryResponseDTO : ProductResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<ProductResponseDTO> Products { get; set; } = new List<ProductResponseDTO>();
    }
}