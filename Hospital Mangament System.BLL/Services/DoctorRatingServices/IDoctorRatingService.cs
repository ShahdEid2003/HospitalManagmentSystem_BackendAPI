using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.DoctorRatingServices
{
    public interface IDoctorRatingService
    {
        Task<DoctorRatingResponse> Create(DoctorRatingRequest request,string userId);

        Task<bool> Update(UpdateDoctorRatingRequest request, string userId);

        Task<bool> Delete( int id,string userId);

        Task<List<DoctorRatingResponse>> GetByDoctor(int doctorId);

        Task<List<DoctorRatingResponse>> GetMyRatings(string userId);

        Task<DoctorRatingResponse> GetById(int id);

        Task<DoctorRatingSummaryResponse> GetRatingSummary(int doctorId);
    }
}
