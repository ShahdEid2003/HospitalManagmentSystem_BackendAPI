using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;
        private readonly IAuthenticationService _authService;
        public AccountController(IAuthenticationService authService, IStringLocalizer<SharedResources> localizer)
        {
            _authService = authService;
            _localizer = localizer;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.Register(request);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var isConfirmed = await _authService.ConfirmEmail(token, userId);
            if (isConfirmed) return Ok(new { message = _localizer["OK"].Value });
            return BadRequest();
        }
        [HttpGet("ApproveDoctor")]
        public async Task<IActionResult> ApproveDoctor(string userId)
        {
            var result = await _authService.ApproveDoctor(userId);

            if (!result)
                return BadRequest("Doctor approval failed.");

            return Ok("Doctor approved successfully.");
        }
        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var result = await _authService.RequestPasswordRest(request);
            if (!result.Success) return BadRequest();
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (!result.Success) return BadRequest();
            return Ok(result);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authService.RefreshTokenAsync();
            if (!result.Success) return Unauthorized();
            return Ok(result);
        }
    }
}
