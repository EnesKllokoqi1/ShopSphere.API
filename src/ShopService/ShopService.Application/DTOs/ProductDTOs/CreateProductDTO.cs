using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Short description cannot exceed 500 characters.")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "SKU is required.")]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Compare price must be greater than zero.")]
        public decimal? CompareAtPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Low stock threshold cannot be negative.")]
        public int? LowStockThreshold { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        [MaxLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
        public string? Brand { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
