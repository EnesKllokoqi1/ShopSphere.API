using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        public string? FirstName { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255)]
        public string? EmailAddress { get; set; }

        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
        public int? Age { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public Gender? Gender { get; set; }
    }
}
