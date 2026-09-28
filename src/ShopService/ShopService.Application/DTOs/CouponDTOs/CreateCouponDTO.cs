using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.CouponDTOs
{
    public class CreateCouponDTO
    {
        [Required(ErrorMessage = "Coupon code is required.")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Code can only contain letters, numbers, '-' and '_'.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [EnumDataType(typeof(DiscountType))]
        public DiscountType DiscountType { get; set; } = DiscountType.Percentage;

        [Range(0.01, 1_000_000, ErrorMessage = "Discount value must be greater than 0.")]
        public decimal DiscountValue { get; set; }

        [Range(0, 1_000_000)]
        public decimal? MinOrderAmount { get; set; }

        [Range(0.01, 1_000_000)]
        public decimal? MaxDiscountAmount { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxUses { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxUsesPerUser { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
