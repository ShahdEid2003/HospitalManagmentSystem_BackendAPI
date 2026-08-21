using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.Admin
{
    public interface IAdminUserService
    {
        Task<List<AdminUserResponse>> GetAllUsers();
        Task<AdminUserResponse?> GetUserById(string id);

        Task<bool> AddUser(AdminUserRequest request);

        Task<bool> UpdateUser(UpdateAdminUserRequest request);

        Task<bool> DeleteUser(string id);

        Task<bool> ActivateUser(string id);

        Task<bool> DeactivateUser(string id);

        Task<bool> AssignRole(AssignRoleRequest request);
    }
}
