using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Mangament_System.BLL.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(
            IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }


        // =========================
        // GET ALL USERS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result =
                await _adminUserService.GetAllUsers();

            return Ok(result);
        }


        // =========================
        // GET USER BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(
            string id)
        {
            var result =
                await _adminUserService.GetUserById(id);

            if (result == null)
                return NotFound("User not found.");

            return Ok(result);
        }


        // =========================
        // ADD USER
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddUser(
            AdminUserRequest request)
        {
            var result =
                await _adminUserService.AddUser(request);

            if (!result)
                return BadRequest(
                    "Could not create user."
                );

            return Ok(
                "User created successfully."
            );
        }


        // =========================
        // UPDATE USER
        // =========================
        [HttpPut]
        public async Task<IActionResult> UpdateUser(
            UpdateAdminUserRequest request)
        {
            var result =
                await _adminUserService.UpdateUser(
                    request
                );

            if (!result)
                return BadRequest(
                    "Could not update user."
                );

            return Ok(
                "User updated successfully."
            );
        }


        // =========================
        // DELETE USER
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(
            string id)
        {
            var result =
                await _adminUserService.DeleteUser(id);

            if (!result)
                return NotFound(
                    "User not found."
                );

            return Ok(
                "User deleted successfully."
            );
        }


        // =========================
        // ACTIVATE USER
        // =========================
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateUser(
            string id)
        {
            var result =
                await _adminUserService.ActivateUser(id);

            if (!result)
                return NotFound(
                    "User not found."
                );

            return Ok(
                "User activated successfully."
            );
        }


        // =========================
        // DEACTIVATE USER
        // =========================
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(
            string id)
        {
            var result =
                await _adminUserService.DeactivateUser(id);

            if (!result)
                return NotFound(
                    "User not found."
                );

            return Ok(
                "User deactivated successfully."
            );
        }


        // =========================
        // ASSIGN ROLE
        // =========================
        [HttpPut("assign-role")]
        public async Task<IActionResult> AssignRole(
            AssignRoleRequest request)
        {
            var result =
                await _adminUserService.AssignRole(
                    request
                );

            if (!result)
                return BadRequest(
                    "Could not assign role."
                );

            return Ok(
                "Role assigned successfully."
            );
        }
    }
}
