using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.AppointmentServices
{
    public interface IAppointmentService
    {
        Task<AppointmentResponse> Create(AppointmentRequest request);

        Task<bool> Update(UpdateAppointmentRequest request);

        Task<bool> Delete(int id);

        Task<List<AppointmentResponse>> GetAll();

        Task<AppointmentResponse> GetById(int id);

        Task<List<AppointmentResponse>> GetTodayAppointments(string userId);
    }
}
