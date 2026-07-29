using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.AppointmentRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.MedicalRecordRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.MedicalRecoredServices
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public MedicalRecordService(
            IMedicalRecordRepository medicalRecordRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
        }
        public async Task<MedicalRecordResponse> Create(MedicalRecordRequest request, string userId)
        {
            var doctor = await _doctorRepository.GetOne(x => x.UserId == userId);

            if (doctor == null)
            {
                throw new Exception("Doctor not found.");
            }

            var patient = await _patientRepository.GetOne(x => x.Id == request.PatientId);

            if (patient == null)
                throw new Exception("Patient not found");

            var appointment = await _appointmentRepository.GetOne( x => x.Id == request.AppointmentId);

            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }
            if (appointment.DoctorId != doctor.Id)
            {
                throw new Exception("This appointment does not belong to you.");
            }

            if (appointment.PatientId != request.PatientId)
            {
                throw new Exception("Invalid patient.");
            }

            var existingRecord = await _medicalRecordRepository.GetOne(
               x => x.AppointmentId == request.AppointmentId);

            if (existingRecord != null)
            {
                throw new Exception("This appointment already has a medical record.");
            }

            var medicalRecord = request.Adapt<MedicalRecord>();

            medicalRecord.DoctorId = doctor.Id;

            await _medicalRecordRepository.Create(medicalRecord);

            medicalRecord = await _medicalRecordRepository.GetOne(
                x => x.Id == medicalRecord.Id,
                new string[]
                {
            $"{nameof(MedicalRecord.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(MedicalRecord.Patient)}.{nameof(Patient.User)}",
            nameof(MedicalRecord.Translations),
            nameof(MedicalRecord.CreatedBy)
                });

            return medicalRecord.Adapt<MedicalRecordResponse>();
        }
        public async Task<bool> Update(UpdateMedicalRecordRequest request)
        {
            var medicalRecord = await _medicalRecordRepository.GetOne(
                x => x.Id == request.Id,
                new string[]
                {
            nameof(MedicalRecord.Translations)
                });

            if (medicalRecord == null)
                return false;

            medicalRecord.VisitDate = request.VisitDate;

            foreach (var translation in request.Translations)
            {
                var existing = medicalRecord.Translations
                    .FirstOrDefault(x => x.Language == translation.Language);

                if (existing != null)
                {
                    existing.Diagnosis = translation.Diagnosis;
                    existing.Notes = translation.Notes;
                }
            }

            return await _medicalRecordRepository.Update(medicalRecord);
        }
        public async Task<bool> Delete(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetOne(x => x.Id == id);

            if (medicalRecord == null)
                return false;

            return await _medicalRecordRepository.Delete(medicalRecord);
        }
        public async Task<List<MedicalRecordResponse>> GetAll()
        {
            var records = await _medicalRecordRepository.GetAll(
                includes: new string[]
                {
            $"{nameof(MedicalRecord.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(MedicalRecord.Patient)}.{nameof(Patient.User)}",
            nameof(MedicalRecord.Translations),
            nameof(MedicalRecord.CreatedBy)
                });

            return records.Adapt<List<MedicalRecordResponse>>();
        }
        public async Task<MedicalRecordResponse> GetById(int id)
        {
            var record = await _medicalRecordRepository.GetOne(
                x => x.Id == id,
                new string[]
                {
            $"{nameof(MedicalRecord.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(MedicalRecord.Patient)}.{nameof(Patient.User)}",
            nameof(MedicalRecord.Translations),
            nameof(MedicalRecord.CreatedBy)
                });

            if (record == null)
                return null;

            return record.Adapt<MedicalRecordResponse>();
        }
        public async Task<List<MedicalRecordResponse>> GetPatientMedicalRecords(int patientId)
        {
            var records = await _medicalRecordRepository.GetAll(
                x => x.PatientId == patientId,
                new string[]
                {
            $"{nameof(MedicalRecord.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(MedicalRecord.Patient)}.{nameof(Patient.User)}",
            nameof(MedicalRecord.Translations),
            nameof(MedicalRecord.CreatedBy)
                });

            return records.Adapt<List<MedicalRecordResponse>>();
        }
        public async Task<List<MedicalRecordResponse>> GetDoctorMedicalRecords(string userId)
        {
            var doctor = await _doctorRepository.GetOne(x => x.UserId == userId);

            if (doctor == null)
                return new List<MedicalRecordResponse>();

            var records = await _medicalRecordRepository.GetAll(
                x => x.DoctorId == doctor.Id,
                new string[]
                {
            $"{nameof(MedicalRecord.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(MedicalRecord.Patient)}.{nameof(Patient.User)}",
            nameof(MedicalRecord.Translations),
            nameof(MedicalRecord.CreatedBy)
                });

            return records.Adapt<List<MedicalRecordResponse>>();
        }
    }
}
