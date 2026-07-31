using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.PrescriptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public PrescriptionController(
            IPrescriptionService prescriptionService,
            IStringLocalizer<SharedResources> localizer)
        {
            _prescriptionService = prescriptionService;
            _localizer = localizer;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionRequest request)
        {
            var result = await _prescriptionService.Create(request);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePrescriptionRequest request)
        {
            var result = await _prescriptionService.Update(request);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _prescriptionService.Delete(id);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _prescriptionService.GetAll());
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _prescriptionService.GetById(id));
        }

        [Authorize(Roles = "Doctor,Receptionist,Admin")]
        [HttpGet("MedicalRecord/{medicalRecordId}")]
        public async Task<IActionResult> GetByMedicalRecord(int medicalRecordId)
        {
            return Ok(await _prescriptionService.GetByMedicalRecord(medicalRecordId));
        }
    }
}
