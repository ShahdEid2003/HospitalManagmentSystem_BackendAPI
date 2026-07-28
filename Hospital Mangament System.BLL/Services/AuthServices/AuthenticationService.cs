using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Hospital_Mangament_System.BLL.Services.Email;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.AuthServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AuthenticationService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }
        public async Task<bool> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
        public async Task<bool> ApproveDoctor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            if (!await _userManager.IsInRoleAsync(user, "Doctor"))
                return false;

            user.IsApproved = true;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            // التحقق من الحقول حسب الدور
            if (request.Role == "Patient")
            {
                if (string.IsNullOrWhiteSpace(request.NationalId) ||
                    request.Age == null ||
                    string.IsNullOrWhiteSpace(request.Gender))
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "National ID, Age and Gender are required for Patient."
                    };
                }
            }
            else if (request.Role == "Doctor")
            {
                if (request.DepartmentId == null ||
                    string.IsNullOrWhiteSpace(request.Specialty) ||
                    string.IsNullOrWhiteSpace(request.LicenseNumber))
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Department, Specialty and License Number are required for Doctor."
                    };
                }
            }

            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser != null)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            var user = request.Adapt<ApplicationUser>();

            user.UserName = request.Email;
            user.IsApproved = request.Role != "Doctor";

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Register Failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            if (request.Role == "Patient")
            {
                var patient = request.Adapt<Patient>();

                patient.UserId = user.Id;
                patient.MedicalRecordNumber = Guid.NewGuid().ToString();

                await _patientRepository.Create(patient);
            }
            else if (request.Role == "Doctor")
            {
                var doctor = request.Adapt<Doctor>();

                doctor.UserId = user.Id;

                await _doctorRepository.Create(doctor);

                var approveUrl =
                    $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                    $"{_httpContextAccessor.HttpContext.Request.Host}" +
                    $"/api/Account/ApproveDoctor?userId={user.Id}";

                await _emailSender.SendDoctorApprovalEmail(
                    "shahdeid012@gmail.com",
                    user.FullName,
                    user.Email,
                    approveUrl);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            token = Uri.EscapeDataString(token);

            var emailUrl =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host}" +
                $"/api/Account/ConfirmEmail?token={token}&userId={user.Id}";

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm Email",
                $"<h2>Welcome {user.FullName}</h2><a href='{emailUrl}'>Confirm Email</a>");

            return new RegisterResponse
            {
                Success = true,
                Message = "Account Created Successfully"
            };
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResponse() { Success = false, Message = "invalid email" };
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse() { Success = false, Message = " email is not confirmed" };

            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse() { Success = false, Message = " user is blocked" };

            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return new LoginResponse() { Success = false, Message = "invalid password" };
            }
            var refreshToken = await GenerateRefreshToken(user);
            SetRefreshTokenCookies(refreshToken);

            return new LoginResponse() { Success = true, Message = "Success", AccessToken = await GenerateAccessToken(user)};
        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
    };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {  
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(15);
            await _userManager.UpdateAsync(user);
            return refreshToken;
        }
        private void SetRefreshTokenCookies(string refreshToken)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,//true for production
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(15)


            });
        }
        public async Task<LoginResponse> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "no refresh token"
                };
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "refresh token Expires"
                };
            }
            var newRefreshToken = await GenerateRefreshToken(user);
            SetRefreshTokenCookies(newRefreshToken);
            return new LoginResponse
            {
                Success = true,
                Message = "Succes",
                AccessToken = await GenerateAccessToken(user),

            };
        }

        public async Task<ForgetPasswordResponse> RequestPasswordRest(ForgetPasswordRequest request)
        {
           
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgetPasswordResponse() { Success = false, message = "Email is not Found" };

            }
            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            user.CodeRestPassword = code;
            user.PasswordRestCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(request.Email, "reset password", $"<p>Code is {code}</p>");
            return new ForgetPasswordResponse() { Success = true, message = "code sent to your email" };
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse() { Success = false, Message = "Email is not Found" };

            }
            else if (request.Code != user.CodeRestPassword)
            {
                return new ResetPasswordResponse() { Success = false, Message = "code is not correct" };

            }
            else if (user.PasswordRestCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse() { Success = false, Message = "code expired" };
            }
            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSamePassword)
            {
                return new ResetPasswordResponse() { Success = false, Message = "New Password Must Be Different From Old Password" };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse() { Success = false, Message = "password reset failed" };
            }
            await _emailSender.SendEmailAsync(request.Email, "change password", "<p> your password is changed </p>");
            return new ResetPasswordResponse() { Success = true, Message = "password reset success" };
        }

    }
}
