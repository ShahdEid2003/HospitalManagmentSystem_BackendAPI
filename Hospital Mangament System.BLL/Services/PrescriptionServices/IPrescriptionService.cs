using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.PrescriptionServices
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponse> Create(PrescriptionRequest request);

        Task<bool> Update(UpdatePrescriptionRequest request);

        Task<bool> Delete(int id);

        Task<List<PrescriptionResponse>> GetAll();

        Task<PrescriptionResponse> GetById(int id);

        Task<List<PrescriptionResponse>> GetByMedicalRecord(int medicalRecordId);
    }
}
