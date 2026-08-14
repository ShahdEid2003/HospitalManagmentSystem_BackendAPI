using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DoctorController(IDoctorService doctorService,IStringLocalizer<SharedResources> localizer)
        {
            _doctorService = doctorService;
            _localizer = localizer;
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("Patients/{patientId}")]
        public async Task<IActionResult> GetPatientDetails( int patientId)
        {
            var userId = User.FindFirstValue( ClaimTypes.NameIdentifier);

            var result = await _doctorService.GetPatientDetails( patientId, userId);

            return Ok(result);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("MyPatients")]
        public async Task<IActionResult> GetMyPatients()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _doctorService.GetMyPatients(userId);

            return Ok(result);
        }
    }
}
