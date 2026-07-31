using ShopService.Application.DTOs;
using ShopService.Application.DTOs.UserDTOs;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDTO?> LogInAsync(UserLoginDTO userLoginDTO);
        Task<UserResponseDTO?> RegisterAsync(UserRegisterDTO userRegisterDTO);
        Task<bool> DeleteUserAsync(Guid guid);
        Task<UserResponseDTO?> UpdateUserAsync(UserUpdateDTO user,Guid guid);
        Task<TokenResponseDTO?> RefreshTokens(string refreshToken);
        Task<VerifyEmailResponseDTO> VerifyEmail(string emailToken);
        Task<bool> LogoutAsync(Guid userId);
        Task<ResendVerificationRequestDTO?> ResendVerificationAsync(string email);

    }
}
