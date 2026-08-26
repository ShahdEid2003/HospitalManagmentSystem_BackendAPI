using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.PatientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient,Admin")]
    public class PatientController : ControllerBase
    {
            private readonly IStringLocalizer _localizer;
            private readonly IPatientService _patientService;
       

        public PatientController(IPatientService patientService, IStringLocalizer<SharedResources> localizer)
            {
                _patientService = patientService;
                _localizer = localizer;
            }
        
            [HttpGet("Doctors")]
            public async Task<IActionResult> GetDoctors()
            {
                var result = await _patientService.GetDoctors();
                return Ok(result);
            }

            [HttpGet("SearchDoctors")]
            public async Task<IActionResult> SearchDoctors(
                [FromQuery] DoctorSearchRequest request)
            {
                var result = await _patientService.SearchDoctors(request);
                return Ok(result);
            }

            [HttpGet("Doctors/{doctorId}")]
            public async Task<IActionResult> GetDoctorDetails(int doctorId)
            {
                var result = await _patientService.GetDoctorDetails(doctorId);
                return Ok(result);
            }

            [HttpGet("Doctors/{doctorId}/Schedule")]
            public async Task<IActionResult> GetDoctorSchedule(int doctorId)
            {
                var result = await _patientService.GetDoctorSchedule(doctorId);
                return Ok(result);
            }

           
        [Authorize(Roles = "Patient")]
        [HttpPost("Bookings")]
        public async Task<IActionResult> BookAppointment( BookAppointmentRequest request)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var result = await _patientService.BookAppointment(
                request,
                userId);

            return Ok(result);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("Bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var result = await _patientService.GetMyBookings(
                userId);

            return Ok(result);
        }

        [HttpGet("Appointments")]
            public async Task<IActionResult> GetMyAppointments()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _patientService.GetMyAppointments(userId);

                return Ok(result);
            }

            [HttpPatch("Appointments/{appointmentId}/Cancel")]
            public async Task<IActionResult> CancelAppointment(int appointmentId)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _patientService.CancelAppointment(
                    appointmentId,
                    userId);

                return Ok(result);
            }

            [HttpGet("MedicalRecords")]
            public async Task<IActionResult> GetMyMedicalRecords()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _patientService.GetMyMedicalRecords(userId);

                return Ok(result);
            }

            [HttpGet("Prescriptions")]
            public async Task<IActionResult> GetMyPrescriptions()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _patientService.GetMyPrescriptions(userId);

                return Ok(result);
            }

            [HttpGet("LabResults")]
            public async Task<IActionResult> GetMyLabResults()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _patientService.GetMyLabResults(userId);

                return Ok(result);
            }
        
    }
}
