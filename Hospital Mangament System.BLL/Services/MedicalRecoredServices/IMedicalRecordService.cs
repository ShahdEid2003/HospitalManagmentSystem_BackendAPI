using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.MedicalRecoredServices
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecordResponse> Create(MedicalRecordRequest request, string userId);

        Task<bool> Update(UpdateMedicalRecordRequest request);

        Task<bool> Delete(int id);

        Task<List<MedicalRecordResponse>> GetAll();

        Task<MedicalRecordResponse> GetById(int id);

        Task<List<MedicalRecordResponse>> GetPatientMedicalRecords(int patientId);

        Task<List<MedicalRecordResponse>> GetDoctorMedicalRecords(string userId);
    }
}
