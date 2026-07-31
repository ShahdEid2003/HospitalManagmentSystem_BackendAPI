using Hospital_Managment_System.DAL.Repository.AppointmentRepositories;
using Hospital_Managment_System.DAL.Repository.DepartmentRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.MedicalRecordRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Hospital_Managment_System.DAL.Repository.PrescriptionRepositories;
using Hospital_Managment_System.DAL.Utiles;
using Hospital_Mangament_System.BLL.Services.AppointmentServices;
using Hospital_Mangament_System.BLL.Services.AuthServices;
using Hospital_Mangament_System.BLL.Services.DepartmentServices;
using Hospital_Mangament_System.BLL.Services.Email;
using Hospital_Mangament_System.BLL.Services.MedicalRecoredServices;
using Hospital_Mangament_System.BLL.Services.PrescriptionServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors.Infrastructure;
using AuthenticationService = Hospital_Mangament_System.BLL.Services.AuthServices.AuthenticationService;
using IAuthenticationService = Hospital_Mangament_System.BLL.Services.AuthServices.IAuthenticationService;

namespace Hospital_Managment_System_.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Services.AddScoped<IDepartmentService, DepartmentService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<IPatientRepository, PatientRepository>();
            Services.AddScoped<IDoctorRepository, DoctorRepository>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            Services.AddScoped<IAppointmentService, AppointmentService>();
            Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            Services.AddScoped<IPrescriptionService, PrescriptionService>();

            return Services;
        }
    }
}
