using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.DepartmentRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DepartmentServices
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _IDepartmentRepository;
        public DepartmentService(IDepartmentRepository IDepartmentRepository)
        {
            _IDepartmentRepository = IDepartmentRepository;
        }
        public async Task<bool> CreateDepartment(DepartmentRequest request)
        {
            var department = request.Adapt<Department>();
            await _IDepartmentRepository.Create(department);
            return true;
            
        }
    }
}
