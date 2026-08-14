using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.DoctorRatingRepositories;
using Hospital_Managment_System.DAL.Repository.DoctorRepositories;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorRatingServices
{
    public class DoctorRatingService : IDoctorRatingService
    {
        private readonly IDoctorRatingRepository _doctorRatingRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public DoctorRatingService(
            IDoctorRatingRepository doctorRatingRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _doctorRatingRepository = doctorRatingRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

      

        public async Task<DoctorRatingResponse> Create(DoctorRatingRequest request,string userId)
        {
            
            var patient = await _patientRepository.GetOne( x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            // Validate rating
            if (request.Rating < 1 || request.Rating > 5)
                throw new Exception(
                    "Rating must be between 1 and 5.");

            // Check doctor
            var doctor = await _doctorRepository.GetOne(
                x => x.Id == request.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            // Check if patient already rated this doctor
            var existingRating =
                await _doctorRatingRepository.GetOne(
                    x => x.DoctorId == request.DoctorId &&
                         x.PatientId == patient.Id);

            if (existingRating != null)
                throw new Exception(
                    "You have already rated this doctor.");

          
            var rating = request.Adapt<DoctorRating>();

            // Set relationships
            rating.DoctorId = doctor.Id;
            rating.PatientId = patient.Id;

            await _doctorRatingRepository.Create(rating);

           
            rating = await _doctorRatingRepository.GetOne(
                x => x.Id == rating.Id,
                new string[]
                {
                    nameof(DoctorRating.Patient),
                    nameof(DoctorRating.Doctor)
                });

          
            var patientWithUser = await _patientRepository.GetOne(
                x => x.Id == rating.PatientId,
                new string[]
                {
                    nameof(Patient.User)
                });

            var response = rating.Adapt<DoctorRatingResponse>();

            response.PatientName =patientWithUser?.User?.FullName;

            return response;
        }


      

        public async Task<bool> Update(UpdateDoctorRatingRequest request,string userId)
        {
            var patient = await _patientRepository.GetOne(
                x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            if (request.Rating < 1 || request.Rating > 5)
                throw new Exception(
                    "Rating must be between 1 and 5.");

            var rating = await _doctorRatingRepository.GetOne(
                x => x.Id == request.Id);

            if (rating == null)
                return false;

            // Make sure this patient owns the rating
            if (rating.PatientId != patient.Id)
                throw new Exception(
                    "You are not allowed to update this rating.");

            rating.Rating = request.Rating;
            rating.Comment = request.Comment;

            return await _doctorRatingRepository.Update(rating);
        }


      
        public async Task<bool> Delete(
            int id,
            string userId)
        {
            var patient = await _patientRepository.GetOne(x => x.UserId == userId);

            if (patient == null)
                throw new Exception("Patient not found.");

            var rating = await _doctorRatingRepository.GetOne(
                x => x.Id == id);

            if (rating == null)
                return false;

            // Only owner can delete
            if (rating.PatientId != patient.Id)
                throw new Exception(
                    "You are not allowed to delete this rating.");

            return await _doctorRatingRepository.Delete(rating);
        }


      

        public async Task<List<DoctorRatingResponse>> GetByDoctor(int doctorId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.Id == doctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var ratings = await _doctorRatingRepository
                .GetQueryable(
                    x => x.DoctorId == doctorId,
                    new string[]
                    {
                        nameof(DoctorRating.Patient),
                        nameof(DoctorRating.Doctor)
                    })
                .Include(x => x.Patient.User)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            return ratings.Adapt<List<DoctorRatingResponse>>();
        }


     

        public async Task<List<DoctorRatingResponse>> GetMyRatings( string userId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.UserId == userId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var ratings = await _doctorRatingRepository
                .GetQueryable(
                    x => x.DoctorId == doctor.Id,
                    new string[]
                    {
                        nameof(DoctorRating.Patient),
                        nameof(DoctorRating.Doctor)
                    })
                .Include(x => x.Patient.User)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            return ratings.Adapt<List<DoctorRatingResponse>>();
        }



        public async Task<DoctorRatingResponse> GetById(int id)
        {
            var rating = await _doctorRatingRepository
                .GetQueryable(
                    x => x.Id == id,
                    new string[]
                    {
                        nameof(DoctorRating.Patient),
                        nameof(DoctorRating.Doctor)
                    })
                .Include(x => x.Patient.User)
                .FirstOrDefaultAsync();

            if (rating == null)
                return null;

            return rating.Adapt<DoctorRatingResponse>();
        }


        public async Task<DoctorRatingSummaryResponse>GetRatingSummary(int doctorId)
        {
            var doctor = await _doctorRepository.GetOne(
                x => x.Id == doctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");

            var ratings = await _doctorRatingRepository.GetAll(
                x => x.DoctorId == doctorId);

            if (ratings == null || ratings.Count == 0)
            {
                return new DoctorRatingSummaryResponse
                {
                    DoctorId = doctorId,
                    AverageRating = 0,
                    TotalRatings = 0
                };
            }

            return new DoctorRatingSummaryResponse
            {
                DoctorId = doctorId,

                AverageRating =Math.Round(ratings.Average(x => x.Rating),1),

                TotalRatings = ratings.Count
            };
        }
    }
    }
