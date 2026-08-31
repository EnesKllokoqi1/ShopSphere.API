using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public Guid? ParentCategoryId { get; set; }

        [Range(0, 999, ErrorMessage = "Display order must be between 0 and 999")]
        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
