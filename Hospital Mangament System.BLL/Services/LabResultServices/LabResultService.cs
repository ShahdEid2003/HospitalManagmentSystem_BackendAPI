using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.LabResultRepositories;
using Hospital_Managment_System.DAL.Repository.MedicalRecordRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.LabResultServices
{
    public class LabResultService:ILabResultService
    {
        private readonly ILabResultRepository _labResultRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public LabResultService(
            ILabResultRepository labResultRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _labResultRepository = labResultRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task<LabResultResponse> Create(LabResultRequest request)
        {
            var medicalRecord = await _medicalRecordRepository.GetOne(
                x => x.Id == request.MedicalRecordId);

            if (medicalRecord == null)
                throw new Exception("Medical Record not found.");

            var labResult = request.Adapt<LabResult>();

            await _labResultRepository.Create(labResult);

            labResult = await _labResultRepository.GetOne(
                x => x.Id == labResult.Id,
                new string[]
                {
                    nameof(LabResult.Translations),
                    nameof(LabResult.CreatedBy)
                });

            return labResult.Adapt<LabResultResponse>();
        }

        public async Task<bool> Update(UpdateLabResultRequest request)
        {
            var labResult = await _labResultRepository.GetOne(
                x => x.Id == request.Id,
                new string[]
                {
                    nameof(LabResult.Translations)
                });

            if (labResult == null)
                return false;

            labResult.ResultDate = request.ResultDate;

            foreach (var translation in request.Translations)
            {
                var existing = labResult.Translations
                    .FirstOrDefault(x => x.Language == translation.Language);

                if (existing != null)
                {
                   
                    existing.TestName = translation.TestName;
                    existing.Result = translation.Result;
                    existing.Notes = translation.Notes;
                }
            }

            return await _labResultRepository.Update(labResult);
        }

        public async Task<bool> Delete(int id)
        {
            var labResult = await _labResultRepository.GetOne(x => x.Id == id);

            if (labResult == null)
                return false;

            return await _labResultRepository.Delete(labResult);
        }

        public async Task<List<LabResultResponse>> GetAll()
        {
            var results = await _labResultRepository.GetAll(
                includes: new string[]
                {
                    nameof(LabResult.Translations),
                    nameof(LabResult.CreatedBy)
                });

            return results.Adapt<List<LabResultResponse>>();
        }

        public async Task<LabResultResponse> GetById(int id)
        {
            var result = await _labResultRepository.GetOne(
                x => x.Id == id,
                new string[]
                {
                    nameof(LabResult.Translations),
                    nameof(LabResult.CreatedBy)
                });

            if (result == null)
                return null;

            return result.Adapt<LabResultResponse>();
        }

        public async Task<List<LabResultResponse>> GetByMedicalRecord(int medicalRecordId)
        {
            var results = await _labResultRepository.GetAll(
                x => x.MedicalRecordId == medicalRecordId,
                new string[]
                {
                    nameof(LabResult.Translations),
                    nameof(LabResult.CreatedBy)
                });

            return results.Adapt<List<LabResultResponse>>();
        }
    }
}
