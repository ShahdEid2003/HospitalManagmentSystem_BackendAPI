using Hospital_Managment_System.DAL.Repository.DepartmentRepositories;
using Hospital_Mangament_System.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Hospital_Managment_System_.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Services.AddScoped<IDepartmentService, DepartmentService>();
          
            return Services;
        }
    }
}
