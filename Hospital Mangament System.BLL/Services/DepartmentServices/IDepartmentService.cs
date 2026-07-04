using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DepartmentServices
{
    public interface IDepartmentService
    {
        public Task<bool> CreateDepartment(DepartmentRequest request);
        Task<List<DepartmentResponse>> GetAll();

        Task<DepartmentResponse?> GetDepartment(Expression<Func<Department, bool>> filiter);


        Task<bool> Update(UpdateDepartmentRequest request);

        Task<bool> Delete(int id);
    }
}
