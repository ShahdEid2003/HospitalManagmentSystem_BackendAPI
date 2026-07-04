using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.DepartmentRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<bool> Delete(int id)
        {
            var department = await _IDepartmentRepository.GetOne(d => d.Id == id);
            if (department == null) return false;
            return await _IDepartmentRepository.Delete(department);
        }

        public async Task<List<DepartmentResponse>> GetAll()
        {
            var departments = await _IDepartmentRepository.GetAll(d=> d.Status == EntityStatus.Active, new string[] { nameof(Department.Translations), nameof(Department.CreatedBy) });
            return departments.Adapt<List<DepartmentResponse>>();
        }

        public async Task<DepartmentResponse?> GetDepartment(Expression<Func<Department, bool>> filiter)
        {
            var brand = await _IDepartmentRepository.GetOne(filiter, new string[] { nameof(Department.Translations), nameof(Department.CreatedBy) });
            if (brand == null) return null;
            return brand.Adapt<DepartmentResponse>(); ;
        }

        public async Task<bool> Update(UpdateDepartmentRequest request)
        {
            var department = await _IDepartmentRepository.GetOne(
                d => d.Id == request.Id,
                new string[]
                {
            nameof(Department.Translations)
                });

            if (department == null)
                return false;
            request.Adapt<Department>();

            foreach (var item in request.Translations)
            {
                var translation = department.Translations
                    .FirstOrDefault(t => t.Language == item.Language);

                if (translation != null)
                {
                    translation.Name = item.Name;
                    translation.Description = item.Description;
                }
                else
                {
                    return false;
                }
            }

            return await _IDepartmentRepository.Update(department);
        }
    }
}
