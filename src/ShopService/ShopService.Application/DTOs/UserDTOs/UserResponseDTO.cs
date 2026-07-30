using ShopService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }         
        public UserRole Role { get; set; }            
        public string? VerificationToken { get; set; }
        public DateTime? EmailVerificationExpiryTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string EmailVerificationStatus => IsEmailVerified ? "Verified" : "Pending Verification";
    }
}
