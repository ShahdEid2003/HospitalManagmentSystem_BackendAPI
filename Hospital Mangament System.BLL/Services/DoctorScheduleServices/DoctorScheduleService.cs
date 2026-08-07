using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorScheduleRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorScheduleServices
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorScheduleService(
            IDoctorScheduleRepository doctorScheduleRepository,
            IDoctorRepository doctorRepository)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorScheduleResponse> Create(DoctorScheduleRequest request,string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            if (request.StartTime >= request.EndTime)
                throw new Exception(
                    "Start time must be before end time.");

            var existingSchedule =
                await _doctorScheduleRepository.GetOne(
                    x => x.DoctorId == doctor.Id &&
                         x.DayOfWeek == request.DayOfWeek);

            if (existingSchedule != null)
                throw new Exception(
                    "Schedule already exists for this day.");

            var schedule = request.Adapt<DoctorSchedule>();

            schedule.DoctorId = doctor.Id;

            await _doctorScheduleRepository.Create(schedule);

            schedule = await _doctorScheduleRepository.GetOne(
                x => x.Id == schedule.Id,
                new string[]
                {
                    nameof(DoctorSchedule.Doctor),
                    nameof(DoctorSchedule.CreatedBy)
                });

            return schedule.Adapt<DoctorScheduleResponse>();
        }

        public async Task<bool> Update(UpdateDoctorScheduleRequest request, string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var schedule = await _doctorScheduleRepository.GetOne(
                x => x.Id == request.Id);

            if (schedule == null)
                return false;

            if (schedule.DoctorId != doctor.Id)
                throw new Exception(
                    "You are not allowed to update this schedule.");

            if (request.StartTime >= request.EndTime)
                throw new Exception(
                    "Start time must be before end time.");

            var duplicate = await _doctorScheduleRepository.GetOne(
                x => x.DoctorId == doctor.Id &&
                     x.DayOfWeek == request.DayOfWeek &&
                     x.Id != request.Id);

            if (duplicate != null)
                throw new Exception(
                    "Schedule already exists for this day.");

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.StartTime = request.StartTime;
            schedule.EndTime = request.EndTime;
            schedule.IsAvailable = request.IsAvailable;

            return await _doctorScheduleRepository.Update(schedule);
        }

        public async Task<bool> Delete(
            int id,
            string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var schedule = await _doctorScheduleRepository.GetOne(
                x => x.Id == id);

            if (schedule == null)
                return false;

            if (schedule.DoctorId != doctor.Id)
                throw new Exception(
                    "You are not allowed to delete this schedule.");

            return await _doctorScheduleRepository.Delete(schedule);
        }

        public async Task<List<DoctorScheduleResponse>> GetMySchedule(
            string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var schedules = await _doctorScheduleRepository.GetAll(
                x => x.DoctorId == doctor.Id,
                new string[]
                {
                    nameof(DoctorSchedule.Doctor),
                    nameof(DoctorSchedule.CreatedBy)
                });

            return schedules
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .Adapt<List<DoctorScheduleResponse>>();
        }

        public async Task<DoctorScheduleResponse> GetById(int id)
        {
            var schedule = await _doctorScheduleRepository.GetOne(
                x => x.Id == id,
                new string[]
                {
                    nameof(DoctorSchedule.Doctor),
                    nameof(DoctorSchedule.CreatedBy)
                });

            if (schedule == null)
                return null;

            return schedule.Adapt<DoctorScheduleResponse>();
        }
    }
}
