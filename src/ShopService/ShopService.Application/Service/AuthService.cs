using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using ShopService.Application.DTOs;
using ShopService.Application.DTOs.UserDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopService.Application.Service
{
    public class AuthService : IAuthService
    {
        public IConfiguration _configuration;
        public IUserRepository _userRepository;
        public AuthService(IConfiguration configuration,IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<bool> DeleteUserAsync(Guid guid)
        {
            var check = await _userRepository.DeleteUser(guid);
            return check;
        }
        public async Task<TokenResponseDTO> GenerateTokens(User user)
        {
            var accessToken = CreateJwtToken(user);
            var refreshToken = await GenerateRefreshToken(user);
            return new TokenResponseDTO {
                AccessToken = accessToken, 
                RefreshToken = refreshToken
            };
        }
        public string CreateJwtToken(User user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            var claims = new List<Claim>()
            { 
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.EmailAddress),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
            };
            if (user.Gender.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.Gender, user.Gender.Value.ToString()));
            }
            var signingCreds = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor =
                new JwtSecurityToken(
                    issuer: _configuration["AppSettings:Issuer"],
                    audience: _configuration["AppSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials:signingCreds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<string> GenerateRefreshToken(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.SaveChanges();
            return refreshToken;
        }
        public async Task<TokenResponseDTO?> LogInAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDTO.EmailAddress);
            if (user is null || !user.IsActive || !user.IsEmailVerified)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(userLoginDTO.Password,user.PasswordHash))
            {
                return null;
            }
          return  await GenerateTokens(user);
        }

        public async Task<TokenResponseDTO?> RefreshTokens(string refreshToken)
        {
            var user = await _userRepository.CheckUserRefreshToken(refreshToken);
            if (user is null)
            {
                return null;
            }
          return  await GenerateTokens(user);
        }

        public async Task<UserResponseDTO?> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            var user = new User
            {
                FirstName = userRegisterDTO.FirstName,
                LastName = userRegisterDTO.LastName,
                EmailAddress = userRegisterDTO.EmailAddress,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password),
                Age = userRegisterDTO.Age,
                PhoneNumber = userRegisterDTO.PhoneNumber,
                Gender = userRegisterDTO.Gender,
                IsActive = true,
                IsEmailVerified = false,
                EmailVerificationToken = GenerateVerificationToken(),
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };
            if (await _userRepository.EmailExistsAsync(userRegisterDTO.EmailAddress))
            {
                return null;
            }
            var createdUser=await _userRepository.CreateUser(user);
            return MapUserDto(createdUser);
        }

        public async Task<UserResponseDTO?> UpdateUserAsync(UserUpdateDTO user,Guid guid)
        {
            var existingUser = await _userRepository.GetUser(guid);
            if (existingUser is null) return null;

            if (!string.IsNullOrEmpty(user.FirstName))
                existingUser.FirstName = user.FirstName;

            if (!string.IsNullOrEmpty(user.LastName))
                existingUser.LastName = user.LastName;

            if (!string.IsNullOrEmpty(user.EmailAddress))
                existingUser.EmailAddress = user.EmailAddress;

            if (user.Age.HasValue)
                existingUser.Age = user.Age.Value;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                existingUser.PhoneNumber = user.PhoneNumber;

            if (user.Gender.HasValue)
                existingUser.Gender = user.Gender.Value;
            var updatedUser = await _userRepository.UpdateUser(existingUser, guid);
            if (updatedUser is null) return null;
            return MapUserDto(updatedUser);
        }

        public async Task<VerifyEmailResponseDTO> VerifyEmail(string emailToken)
        {
            var userWithEmailToken = await _userRepository.CheckEmailVerificationToken(emailToken);
            if (userWithEmailToken is null)
            {
                return new VerifyEmailResponseDTO
                {
                    Success = false,
                    Message="Invalid Token",
                };
            }
            if (userWithEmailToken.IsEmailVerified is true)
            {
                return new VerifyEmailResponseDTO
                {
                    Success = true,
                    Message = "Already Verified"
                };
            }
            if (userWithEmailToken.EmailVerificationTokenExpiry<=DateTime.UtcNow)
            {
                return new VerifyEmailResponseDTO
                {
                    Success = false,
                    Message = "Verification token has expired. Please request a new one.",
                  
                };
            }
            userWithEmailToken.IsEmailVerified = true;
            userWithEmailToken.EmailVerificationToken = null;
            userWithEmailToken.EmailVerificationTokenExpiry = null;
            await _userRepository.SaveChanges();
            return new VerifyEmailResponseDTO
            {
                Success = true,
                Message = "Email verified successfully! You can now log in.",
                Email = userWithEmailToken.EmailAddress,
                VerifiedAt = DateTime.UtcNow
            };

        }
        private UserResponseDTO MapUserDto(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Role = user.Role,
                IsActive = user.IsActive,
                IsEmailVerified = user.IsEmailVerified,
                VerificationToken=user.EmailVerificationToken,
                EmailVerificationExpiryTime=user.EmailVerificationTokenExpiry,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt 
            };
        }
        private string GenerateVerificationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, 32);
        }

        public async Task<ResendVerificationResponseDTO?> ResendVerificationAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return null;
            }
            user.EmailVerificationToken = GenerateVerificationToken();
            user.IsEmailVerified = false;
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _userRepository.SaveChanges();
            return new ResendVerificationResponseDTO
            {
                Success = true,
                Message = "New email verification token generated!",
                Email = user.EmailAddress,
                VerificationToken = user.EmailVerificationToken,
            };
        }

        public async Task<bool> LogoutAsync(Guid userId)
        {
            var user = await _userRepository.GetUser(userId);
            if(user is null)
            {
                return false;
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.SaveChanges();
            return true;
        }
    }
}
