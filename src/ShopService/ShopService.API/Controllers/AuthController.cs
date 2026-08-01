using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs;
using ShopService.Application.DTOs.UserDTOs;
using ShopService.Application.Interfaces;
using System.Security.Claims;

namespace ShopService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<UserResponseDTO?>> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var user = await _authService.RegisterAsync(userRegisterDTO);
            if (user is null)
            {
                return Conflict(new
                {
                    Message = "A user with this email address already exists."
                });
            }

            return Ok(new
            {
                Message = "User has been successfully created.",
                User = user
            });
        }

        [HttpPost("login-user")]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDTO userLoginDTO)
        {
            var tokens = await _authService.LogInAsync(userLoginDTO);
            if (tokens is null)
            {
                return BadRequest(new
                {
                    Message = "Invalid credentials or account is not verified."
                });
            }

            CreateCookies(tokens);
            return Ok(new
            {
                Message = "User has logged in successfully."
            });
        }

        [Authorize]
        [HttpPost("logout-user")]
        public async Task<ActionResult> LogOutUser()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            var check = await _authService.LogoutAsync(userId);
            if (!check)
            {
                return NotFound(new
                {
                    Message = "User was not found."
                });
            }

            DeleteCookies();
            return Ok(new
            {
                Message = "User has logged out successfully."
            });
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            var check = await _authService.DeleteUserAsync(userId);
            if (!check)
            {
                return NotFound(new
                {
                    Message = "User was not found."
                });
            }

            DeleteCookies();
            return Ok(new { Message = "User account deleted successfully." });
        }

        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult<UserResponseDTO?>> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            var user = await _authService.UpdateUserAsync(userUpdateDTO, userId);
            if (user is null)
            {
                return NotFound(new
                {
                    Message = "User was not found."
                });
            }

            return Ok(new
            {
                Message = "User details updated successfully.",
                User = user
            });
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<VerifyEmailResponseDTO>> VerifyEmail([FromBody] VerifyEmailRequestDTO verifyEmailRequestDTO)
        {
            var response = await _authService.VerifyEmail(verifyEmailRequestDTO.EmailVerificationToken);
            return Ok(response);
        }

        [HttpPost("resend-verification-token")]
        public async Task<ActionResult<ResendVerificationResponseDTO?>> ResendVerificationToken([FromBody] ResendVerificationRequestDTO resendVerificationRequestDTO)
        {
            var user = await _authService.ResendVerificationAsync(resendVerificationRequestDTO.EmailAddress);
            if (user is null)
            {
                return NotFound(new
                {
                    Message = "User was not found."
                });
            }

            return Ok(user);
        }

        [HttpPost("refresh-tokens")]
        public async Task<ActionResult> RefreshTokens()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { Message = "A valid session cookie is required to refresh tokens." });
            }

            var tokens = await _authService.RefreshTokens(refreshToken);
            if (tokens is null)
            {
                DeleteCookies();
                return Unauthorized(new { Message = "Invalid or expired refresh token." });
            }

            CreateCookies(tokens);
            return Ok(new
            {
                Message = "Tokens generated successfully."
            });
        }

        #region Helper Methods

        private bool TryGetUserId(out Guid userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out userId);
        }

        private void CreateCookies(TokenResponseDTO tokenResponseDTO)
        {
            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Secure = true
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                Path = "/api/auth/refresh-tokens"
            };

            Response.Cookies.Append("access_token", tokenResponseDTO.AccessToken, accessCookieOptions);
            Response.Cookies.Append("refresh_token", tokenResponseDTO.RefreshToken, refreshCookieOptions);
        }

        private void DeleteCookies()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/api/auth/refresh-tokens",
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
        }

        #endregion
    }
}