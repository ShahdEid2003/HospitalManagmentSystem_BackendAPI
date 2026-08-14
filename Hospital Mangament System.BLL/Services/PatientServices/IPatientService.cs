using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.PatientServices
{
    public interface IPatientService
    {
        Task<List<DoctorResponse>> GetDoctors();
        Task<List<DoctorResponse>> SearchDoctors(DoctorSearchRequest request);
        Task<DoctorDetailsResponse> GetDoctorDetails(int doctorId);
        Task<List<DoctorScheduleResponse>> GetDoctorSchedule(int doctorId);
        Task<BookingResponse> BookAppointment(BookAppointmentRequest request, string userId);
        Task<List<PatientAppointmentResponse>> GetMyAppointments(string userId);

        Task<List<BookingResponse>> GetMyBookings(string userId);
        Task<bool> CancelAppointment(int appointmentId, string userId);
        Task<List<PatientMedicalRecordResponse>> GetMyMedicalRecords(string userId);
        Task<List<PrescriptionResponse>> GetMyPrescriptions(string userId);
        Task<List<LabResultResponse>> GetMyLabResults(string userId);
    }
}
