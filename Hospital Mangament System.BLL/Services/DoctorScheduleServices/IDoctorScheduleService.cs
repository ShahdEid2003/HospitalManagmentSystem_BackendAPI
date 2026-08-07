using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorScheduleServices
{
    public interface IDoctorScheduleService
    {
        Task<DoctorScheduleResponse> Create( DoctorScheduleRequest request,string userId);

        Task<bool> Update( UpdateDoctorScheduleRequest request, string userId);

        Task<bool> Delete(int id, string userId);

        Task<List<DoctorScheduleResponse>> GetMySchedule(string userId);

        Task<DoctorScheduleResponse> GetById(int id);
    }
}
