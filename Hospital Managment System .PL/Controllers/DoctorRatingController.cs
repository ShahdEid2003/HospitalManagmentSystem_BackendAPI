using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Mangament_System.BLL.Services.DoctorRatingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorRatingController : ControllerBase
    {
        private readonly IDoctorRatingService _doctorRatingService;

        public DoctorRatingController(
            IDoctorRatingService doctorRatingService)
        {
            _doctorRatingService = doctorRatingService;
        }


        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create(
            DoctorRatingRequest request)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var result =
                await _doctorRatingService.Create(
                    request,
                    userId);

            return Ok(result);
        }


        [Authorize(Roles = "Patient")]
        [HttpPatch]
        public async Task<IActionResult> Update(
            UpdateDoctorRatingRequest request)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var result =
                await _doctorRatingService.Update(
                    request,
                    userId);

            return Ok(result);
        }



        [Authorize(Roles = "Patient")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var result =
                await _doctorRatingService.Delete(
                    id,
                    userId);

            return Ok(result);
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("MyRatings")]
        public async Task<IActionResult> GetMyRatings()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var result =
                await _doctorRatingService.GetMyRatings(
                    userId);

            return Ok(result);
        }


        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("Doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(
            int doctorId)
        {
            var result =
                await _doctorRatingService.GetByDoctor(
                    doctorId);

            return Ok(result);
        }


        [Authorize(Roles = "Doctor,Admin,Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var result =
                await _doctorRatingService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [Authorize(Roles = "Doctor,Admin,Patient")]
        [HttpGet("Doctor/{doctorId}/Summary")]
        public async Task<IActionResult> GetRatingSummary(
            int doctorId)
        {
            var result =
                await _doctorRatingService
                    .GetRatingSummary(doctorId);

            return Ok(result);
        }
    }
}
