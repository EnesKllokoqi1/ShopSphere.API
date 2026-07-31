using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs;
using ShopService.Application.DTOs.UserDTOs;
using ShopService.Application.Interfaces;

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
        public async Task<ActionResult<UserResponseDTO?>> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPost("login-user")]
        public async Task<ActionResult> LoginUser(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }
        [HttpPost("logout-user")]
        public async Task<ActionResult> LogOutUser()
        {
            throw new NotImplementedException();
        }
        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser()
        {
            throw new NotImplementedException();
        }
        [HttpPut("update-user")]
        public async Task<ActionResult<UserResponseDTO?>> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            throw new NotImplementedException();
        }
        [HttpPost("verify-email")]
        public async Task<ActionResult<VerifyEmailResponseDTO>> VerifyEmail(string emailToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost("resend-verification-token")]
        public async Task<ActionResult<ResendVerificationRequestDTO?>> ResendVerificationToken(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPost("refresh-tokens")]
        public async Task<ActionResult> RefreshTokens()
        {
            throw new NotImplementedException();
        }

        private  void CreateCookies(TokenResponseDTO tokenResponseDTO)
        {
            throw new NotImplementedException();
        }
        private  void DeleteCookies()
        {
            throw new NotImplementedException();
        }
    }
}
