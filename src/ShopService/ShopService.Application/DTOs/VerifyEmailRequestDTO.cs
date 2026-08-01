using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.DTOs
{
    public class VerifyEmailRequestDTO
    {
        public string EmailVerificationToken { get; set; }  = string.Empty;
    }
}
