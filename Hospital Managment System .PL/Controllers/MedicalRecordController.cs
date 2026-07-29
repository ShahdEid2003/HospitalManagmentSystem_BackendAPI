using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.MedicalRecoredServices;
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
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService,
            IStringLocalizer<SharedResources> localizer)
        {
            _medicalRecordService = medicalRecordService;
            _localizer = localizer;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create(MedicalRecordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _medicalRecordService.Create(request, userId);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPatch]
        public async Task<IActionResult> Update(UpdateMedicalRecordRequest request)
        {
            var result = await _medicalRecordService.Update(request);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _medicalRecordService.Delete(id);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _medicalRecordService.GetAll());
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _medicalRecordService.GetById(id));
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("MyRecords")]
        public async Task<IActionResult> MyRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(await _medicalRecordService.GetDoctorMedicalRecords(userId));
        }

        [Authorize(Roles = "Doctor,Receptionist,Admin")]
        [HttpGet("Patient/{patientId}")]
        public async Task<IActionResult> PatientRecords(int patientId)
        {
            return Ok(await _medicalRecordService.GetPatientMedicalRecords(patientId));
        }
    }
}
