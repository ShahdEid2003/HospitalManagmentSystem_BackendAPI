using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<PatientDetailsResponse> GetPatientDetails(int patientId,string userId);
        Task<List<PatientDetailsResponse>> GetMyPatients(string userId);
    }
}
