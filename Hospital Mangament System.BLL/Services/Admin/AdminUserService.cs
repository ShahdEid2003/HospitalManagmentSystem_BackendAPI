using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // =========================
        // GET ALL USERS
        // =========================
        public async Task<List<AdminUserResponse>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            var result = new List<AdminUserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new AdminUserResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,

                    IsActive = !await _userManager.IsLockedOutAsync(user),

                    Roles = roles
                });
            }

            return result;
        }


        // =========================
        // GET USER BY ID
        // =========================
        public async Task<AdminUserResponse?> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new AdminUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                IsActive = !await _userManager.IsLockedOutAsync(user),

                Roles = roles
            };
        }


        // =========================
        // ADD USER
        // =========================
        public async Task<bool> AddUser(AdminUserRequest request)
        {
            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                return false;

            // التأكد أن الـ Role موجود
            if (!await _roleManager.RoleExistsAsync(request.Role))
                return false;

            var user = new ApplicationUser
            {
                FullName = request.FullName,

                Email = request.Email,

                UserName = request.Email,

                PhoneNumber = request.PhoneNumber,

                // Admin هو الذي أنشأ الحساب
                EmailConfirmed = true,

                IsApproved = true
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password
            );

            if (!result.Succeeded)
                return false;

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    request.Role
                );

            if (!roleResult.Succeeded)
            {
                // إذا فشل الـ Role نحذف المستخدم
                await _userManager.DeleteAsync(user);

                return false;
            }

            return true;
        }


        // =========================
        // UPDATE USER
        // =========================
        public async Task<bool> UpdateUser(
            UpdateAdminUserRequest request)
        {
            var user =
                await _userManager.FindByIdAsync(request.Id);

            if (user == null)
                return false;

            // التأكد أن الإيميل الجديد غير مستخدم
            var emailUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (emailUser != null &&
                emailUser.Id != request.Id)
            {
                return false;
            }

            user.FullName = request.FullName;

            user.Email = request.Email;

            user.UserName = request.Email;

            user.PhoneNumber = request.PhoneNumber;

            var result =
                await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }


        // =========================
        // DELETE USER
        // =========================
        public async Task<bool> DeleteUser(string id)
        {
            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var result =
                await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }


        // =========================
        // ACTIVATE USER
        // =========================
        public async Task<bool> ActivateUser(string id)
        {
            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            await _userManager.SetLockoutEnabledAsync(
                user,
                true
            );

            await _userManager.SetLockoutEndDateAsync(
                user,
                null
            );

            return true;
        }


        // =========================
        // DEACTIVATE USER
        // =========================
        public async Task<bool> DeactivateUser(string id)
        {
            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            await _userManager.SetLockoutEnabledAsync(
                user,
                true
            );

            await _userManager.SetLockoutEndDateAsync(
                user,
                DateTimeOffset.UtcNow.AddYears(100)
            );

            return true;
        }


        // =========================
        // ASSIGN ROLE
        // =========================
        public async Task<bool> AssignRole(
            AssignRoleRequest request)
        {
            var user =
                await _userManager.FindByIdAsync(
                    request.UserId
                );

            if (user == null)
                return false;

            // التأكد أن الـ Role موجود
            if (!await _roleManager.RoleExistsAsync(
                    request.Role))
            {
                return false;
            }

            // الأدوار الحالية
            var currentRoles =
                await _userManager.GetRolesAsync(user);

            // إزالة الأدوار القديمة
            if (currentRoles.Any())
            {
                var removeResult =
                    await _userManager.RemoveFromRolesAsync(
                        user,
                        currentRoles
                    );

                if (!removeResult.Succeeded)
                    return false;
            }

            // إضافة الدور الجديد
            var result =
                await _userManager.AddToRoleAsync(
                    user,
                    request.Role
                );

            return result.Succeeded;
        }
    }
}
