using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.AppointmentRepositories;
using Hospital_Managment_System.DAL.Repository.BookingRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorScheduleRepositories;
using Hospital_Managment_System.DAL.Repository.LabResultRepositories;
using Hospital_Managment_System.DAL.Repository.MedicalRecordRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Hospital_Managment_System.DAL.Repository.PrescriptionRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ILabResultRepository _labResultRepository;
        private readonly IAppointmentBookingRepository _appointmentBookingRepository;

        public PatientService(
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IDoctorScheduleRepository doctorScheduleRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IPrescriptionRepository prescriptionRepository,
            ILabResultRepository labResultRepository,
            IAppointmentBookingRepository appointmentBookingRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _doctorScheduleRepository = doctorScheduleRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _prescriptionRepository = prescriptionRepository;
            _labResultRepository = labResultRepository;
            _appointmentBookingRepository = appointmentBookingRepository;
        }

        public async Task<List<DoctorResponse>> GetDoctors()
        {
            var doctors = await _doctorRepository.GetQueryable(
                x => x.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Doctor.User),
                    nameof(Doctor.Department)
                })
                .Include(x => x.Department.Translations)
                .ToListAsync();

            return doctors.Adapt<List<DoctorResponse>>();
        }

        public async Task<List<DoctorResponse>> SearchDoctors(DoctorSearchRequest request)
        {
            var doctors = await _doctorRepository.GetQueryable(
                x => x.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Doctor.User),
                    nameof(Doctor.Department)
                })
                .Include(x => x.Department.Translations)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                doctors = doctors.Where(x =>
                    x.User.FullName.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                    x.Specialty.Contains(request.Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (request.DepartmentId.HasValue)
            {
                doctors = doctors.Where(x =>
                    x.DepartmentId == request.DepartmentId.Value)
                    .ToList();
            }

            return doctors.Adapt<List<DoctorResponse>>();
        }

        public async Task<DoctorDetailsResponse> GetDoctorDetails(int doctorId)
        {
            var doctor = await _doctorRepository.GetQueryable(
                x => x.Id == doctorId && x.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Doctor.User),
                    nameof(Doctor.Department),
                    nameof(Doctor.DoctorRatings)
                })
                .Include(x => x.Department.Translations)
                .FirstOrDefaultAsync();

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var response = doctor.Adapt<DoctorDetailsResponse>();
            var ratings = doctor.DoctorRatings;

            response.TotalRatings = ratings.Count;
            response.AverageRating = ratings.Count == 0
                ? 0
                : Math.Round(ratings.Average(x => x.Rating), 1);

            return response;
        }

        public async Task<List<DoctorScheduleResponse>> GetDoctorSchedule(int doctorId)
        {
            var doctor = await _doctorRepository.GetOne(x => x.Id == doctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var schedules = await _doctorScheduleRepository.GetAll(
                x => x.DoctorId == doctorId && x.IsAvailable);

            return schedules
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .Adapt<List<DoctorScheduleResponse>>();
        }

        public async Task<BookingResponse> BookAppointment(BookAppointmentRequest request, string userId)
        {
            var patient = await _patientRepository.GetOne( x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var doctor = await _doctorRepository.GetOne(
                x => x.Id == request.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            if (request.EndTime <= request.StartTime)
                throw new Exception("End time must be after start time.");

            var existingBooking = await _appointmentBookingRepository.GetOne(
                x => x.DoctorId == request.DoctorId &&
                     x.AppointmentDate == request.AppointmentDate &&
                     x.StartTime == request.StartTime &&
                     x.Status == BookingStatus.Pending);

            if (existingBooking != null)
                throw new Exception("This time already has a pending booking.");

            var booking = new AppointmentBooking
            {
                PatientId = patient.Id,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = BookingStatus.Pending
            };

            await _appointmentBookingRepository.Create(booking);

            booking = await _appointmentBookingRepository.GetOne(
                x => x.Id == booking.Id,
                new string[]
                {
            $"{nameof(AppointmentBooking.Patient)}.{nameof(Patient.User)}",
            $"{nameof(AppointmentBooking.Doctor)}.{nameof(Doctor.User)}"
                });

            return booking.Adapt<BookingResponse>();
        }
        public async Task<List<BookingResponse>> GetMyBookings(string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var bookings = await _appointmentBookingRepository.GetAll(
                x => x.PatientId == patient.Id,
                new string[]
                {
            $"{nameof(AppointmentBooking.Patient)}.{nameof(Patient.User)}",
            $"{nameof(AppointmentBooking.Doctor)}.{nameof(Doctor.User)}"
                });

            return bookings.Adapt<List<BookingResponse>>();
        }
       

        public async Task<List<PatientAppointmentResponse>> GetMyAppointments(string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var appointments = await _appointmentRepository.GetQueryable(
                x => x.PatientId == patient.Id,
                new string[]
                {
            nameof(Appointment.Doctor)
                })
                .Include(x => x.Doctor.User)
                .OrderBy(x => x.AppointmentDate)
                .ThenBy(x => x.StartTime)
                .ToListAsync();

            return appointments.Adapt<List<PatientAppointmentResponse>>();
        }

        public async Task<bool> CancelAppointment(int appointmentId, string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var appointment = await _appointmentRepository.GetOne(
                x => x.Id == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (appointment.PatientId != patient.Id)
                throw new Exception("You are not allowed to cancel this appointment.");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Appointment is already cancelled.");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Completed appointment cannot be cancelled.");

            var appointmentDateTime = appointment.AppointmentDate.ToDateTime(
                appointment.StartTime);

            if (appointmentDateTime <= DateTime.Now)
                throw new Exception("Past appointments cannot be cancelled.");

            appointment.Status = AppointmentStatus.Cancelled;

            return await _appointmentRepository.Update(appointment);
        }

        public async Task<List<PatientMedicalRecordResponse>> GetMyMedicalRecords(
            string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var records = await _medicalRecordRepository
            .GetQueryable(
                x => x.PatientId == patient.Id,
                new string[]
                {
                    nameof(MedicalRecord.Doctor),
                    nameof(MedicalRecord.Translations)
                })
            .Include(x => x.Doctor.User)
            .OrderByDescending(x => x.VisitDate)
            .ToListAsync();

            return records.Adapt<List<PatientMedicalRecordResponse>>();
        }

        public async Task<List<PrescriptionResponse>> GetMyPrescriptions(string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var prescriptions = await _prescriptionRepository
                .GetQueryable(
                    x => x.MedicalRecord.PatientId == patient.Id,
                    new string[]
                    {
                        nameof(Prescription.MedicalRecord),
                         nameof(Prescription.Translations)
                    })
                .Include(x => x.MedicalRecord.Doctor.User)
                .OrderByDescending(x => x.MedicalRecord.VisitDate)
                .ToListAsync();

            return prescriptions.Adapt<List<PrescriptionResponse>>();
        }

        public async Task<List<LabResultResponse>> GetMyLabResults(string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var results = await _labResultRepository
                .GetQueryable(
                    x => x.MedicalRecord.PatientId == patient.Id,
                    new string[]
                    {
                        nameof(LabResult.MedicalRecord)
                    })
                .Include(x => x.MedicalRecord.Doctor.User)
                .OrderByDescending(x => x.MedicalRecord.VisitDate)
                .ToListAsync();

            return results.Adapt<List<LabResultResponse>>();
        }

       
    }
}

