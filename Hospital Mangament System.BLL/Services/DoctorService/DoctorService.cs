using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.AppointmentRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorService
{
    public class DoctorService:IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository, IAppointmentRepository appointmentRepository)
            {
                _doctorRepository = doctorRepository;
                _patientRepository = patientRepository;
                _appointmentRepository = appointmentRepository;
        }
        public async Task<PatientDetailsResponse> GetPatientDetails(int patientId,string userId)
        {
           
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var patient = await _patientRepository.GetOne( x => x.Id == patientId, new string[] {nameof(Patient.User)});

            if (patient == null)
                throw new Exception("Patient not found.");

            return new PatientDetailsResponse
            {
                Id = patient.Id,

                FullName = patient.User.FullName,

                Email = patient.User.Email,

                PhoneNumber = patient.User.PhoneNumber,

                NationalId = patient.NationalId,

                Age = patient.Age,

                Gender = patient.Gender,

                MedicalRecordNumber =patient.MedicalRecordNumber
            };
        }
        public async Task<List<PatientDetailsResponse>> GetMyPatients( string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var appointments = await _appointmentRepository
                .GetQueryable(
                    x => x.DoctorId == doctor.Id,
                    new string[]
                    {
                nameof(Appointment.Patient)
                    })
                .Include(x => x.Patient.User)
                .ToListAsync();

            var patients = appointments
                .Where(x => x.Patient != null)
                .Select(x => x.Patient)
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToList();

            return patients.Adapt<List<PatientDetailsResponse>>();
        }
    }

}
