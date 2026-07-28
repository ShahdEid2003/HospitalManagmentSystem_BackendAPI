using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.AppointmentRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.AppointmentServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }
        public async Task<AppointmentResponse> Create(AppointmentRequest request)
        {
            var appointment = request.Adapt<Appointment>();

            appointment.Status = AppointmentStatus.Pending;

            await _appointmentRepository.Create(appointment);

            appointment = await _appointmentRepository.GetOne(
                x => x.Id == appointment.Id,
                new string[]
                {
            $"{nameof(Appointment.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(Appointment.Patient)}.{nameof(Patient.User)}",
            nameof(Appointment.Translations),
            nameof(Appointment.CreatedBy)
                });

            return appointment.Adapt<AppointmentResponse>();
        }

        public async Task<bool> Delete(int id)
        {
            var appointment = await _appointmentRepository.GetOne(x => x.Id == id);

            if (appointment == null)
                return false;

            return await _appointmentRepository.Delete(appointment);
        }

        public async Task<List<AppointmentResponse>> GetAll()
        {
            var appointments = await _appointmentRepository.GetAll(
                includes: new string[]
                {
            $"{nameof(Appointment.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(Appointment.Patient)}.{nameof(Patient.User)}",
            nameof(Appointment.Translations),
            nameof(Appointment.CreatedBy)
                });

            return appointments.Adapt<List<AppointmentResponse>>();
        }

        public async Task<AppointmentResponse> GetById(int id)
        {
            var appointment = await _appointmentRepository.GetOne(
                x => x.Id == id,
                new string[]
                {
            $"{nameof(Appointment.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(Appointment.Patient)}.{nameof(Patient.User)}",
            nameof(Appointment.Translations),
            nameof(Appointment.CreatedBy)
                });

            if (appointment == null)
                return null;

            return appointment.Adapt<AppointmentResponse>();
        }
        public async Task<List<AppointmentResponse>> GetTodayAppointments(string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                d => d.UserId == userId);

            if (doctor == null)
                return new List<AppointmentResponse>();

            var appointments = await _appointmentRepository.GetAll(
                x => x.DoctorId == doctor.Id &&
                     x.AppointmentDate == DateOnly.FromDateTime(DateTime.Today),
                new string[]
                {
            $"{nameof(Appointment.Doctor)}.{nameof(Doctor.User)}",
            $"{nameof(Appointment.Patient)}.{nameof(Patient.User)}",
            nameof(Appointment.Translations),
            nameof(Appointment.CreatedBy)
                });

            return appointments.Adapt<List<AppointmentResponse>>();
        }
        public async Task<bool> Update(UpdateAppointmentRequest request)
        {
            var appointment = await _appointmentRepository.GetOne(
                x => x.Id == request.Id,
                new string[]
                {
            nameof(Appointment.Translations)
                });

            if (appointment == null)
                return false;

            appointment.AppointmentDate = request.AppointmentDate;
            appointment.StartTime = request.StartTime;
            appointment.EndTime = request.EndTime;
            appointment.Status = request.Status;

            foreach (var translation in request.Translations)
            {
                var existing = appointment.Translations
                    .FirstOrDefault(x => x.Language == translation.Language);

                if (existing == null)
                {
                    appointment.Translations.Add(new AppointmentTranslation
                    {
                        Language = translation.Language,
                        Notes = translation.Notes
                    });
                }
                else
                {
                    existing.Notes = translation.Notes;
                }
            }

            return await _appointmentRepository.Update(appointment);
        }
    }
}
