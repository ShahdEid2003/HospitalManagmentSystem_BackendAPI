using Hospital_Managment_System.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DepartmentServices
{
    public interface IDepartmentService
    {
        public Task<bool> CreateDepartment(DepartmentRequest request);
    }
}
