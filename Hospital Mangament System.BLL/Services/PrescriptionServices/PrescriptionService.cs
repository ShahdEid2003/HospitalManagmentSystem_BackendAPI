using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.MedicalRecordRepositories;
using Hospital_Managment_System.DAL.Repository.PrescriptionRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.PrescriptionServices
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }
        public async Task<PrescriptionResponse> Create(PrescriptionRequest request)
        {
            var medicalRecord = await _medicalRecordRepository.GetOne(
                x => x.Id == request.MedicalRecordId);

            if (medicalRecord == null)
                throw new Exception("Medical Record not found.");

            var prescription = request.Adapt<Prescription>();

            await _prescriptionRepository.Create(prescription);

            prescription = await _prescriptionRepository.GetOne(
                x => x.Id == prescription.Id,
                new string[]
                {
            nameof(Prescription.Translations),
            nameof(Prescription.CreatedBy)
                });

            return prescription.Adapt<PrescriptionResponse>();
        }
        public async Task<bool> Update(UpdatePrescriptionRequest request)
        {
            var prescription = await _prescriptionRepository.GetOne(
                x => x.Id == request.Id,
                new string[]
                {
            nameof(Prescription.Translations)
                });

            if (prescription == null)
                return false;

            foreach (var translation in request.Translations)
            {
                var existing = prescription.Translations
                    .FirstOrDefault(x => x.Language == translation.Language);

                if (existing != null)
                {
                    
                    existing.MedicationName = translation.MedicationName;
                    existing.Dosage = translation.Dosage;
                    existing.Instructions = translation.Instructions;
                }
            }

            return await _prescriptionRepository.Update(prescription);
        }
        public async Task<bool> Delete(int id)
        {
            var prescription = await _prescriptionRepository.GetOne(x => x.Id == id);

            if (prescription == null)
                return false;

            return await _prescriptionRepository.Delete(prescription);
        }
        public async Task<List<PrescriptionResponse>> GetAll()
        {
            var prescriptions = await _prescriptionRepository.GetAll(
                includes: new string[]
                {
            nameof(Prescription.Translations),
            nameof(Prescription.CreatedBy)
                });

            return prescriptions.Adapt<List<PrescriptionResponse>>();
        }
        public async Task<PrescriptionResponse> GetById(int id)
        {
            var prescription = await _prescriptionRepository.GetOne(
                x => x.Id == id,
                new string[]
                {
            nameof(Prescription.Translations),
            nameof(Prescription.CreatedBy)
                });

            if (prescription == null)
                return null;

            return prescription.Adapt<PrescriptionResponse>();
        }
        public async Task<List<PrescriptionResponse>> GetByMedicalRecord(int medicalRecordId)
        {
            var prescriptions = await _prescriptionRepository.GetAll(
                x => x.MedicalRecordId == medicalRecordId,
                new string[]
                {
            nameof(Prescription.Translations),
            nameof(Prescription.CreatedBy)
                });

            return prescriptions.Adapt<List<PrescriptionResponse>>();
        }
    }
}
