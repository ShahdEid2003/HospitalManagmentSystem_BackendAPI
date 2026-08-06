using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.LabResultServices
{
    public interface ILabResultService
    {
        Task<LabResultResponse> Create(LabResultRequest request);

        Task<bool> Update(UpdateLabResultRequest request);

        Task<bool> Delete(int id);

        Task<List<LabResultResponse>> GetAll();

        Task<LabResultResponse> GetById(int id);

        Task<List<LabResultResponse>> GetByMedicalRecord(int medicalRecordId);
    }
}
