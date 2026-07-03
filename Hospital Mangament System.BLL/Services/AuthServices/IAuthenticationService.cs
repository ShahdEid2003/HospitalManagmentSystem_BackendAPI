using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.AuthServices
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> Register(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> ConfirmEmail(string token, string userId);
        Task<bool> ApproveDoctor(string userId);
        //Task<ForgetPasswordResponse> RequestPasswordRest(ForgetPasswordRequest request);
        //Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        //Task<LoginResponse> RefreshTokenAsync();
    }
}
