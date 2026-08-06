using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Mangament_System.BLL.Services.LabResultServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabResultController : ControllerBase
    {
        private readonly ILabResultService _labResultService;

        public LabResultController(ILabResultService labResultService)
        {
            _labResultService = labResultService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create(LabResultRequest request)
        {
            return Ok(await _labResultService.Create(request));
        }

        [Authorize(Roles = "Doctor")]
        [HttpPatch]
        public async Task<IActionResult> Update(UpdateLabResultRequest request)
        {
            return Ok(await _labResultService.Update(request));
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _labResultService.Delete(id));
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _labResultService.GetAll());
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _labResultService.GetById(id));
        }

        [Authorize(Roles = "Doctor,Receptionist,Admin")]
        [HttpGet("MedicalRecord/{medicalRecordId}")]
        public async Task<IActionResult> GetByMedicalRecord(int medicalRecordId)
        {
            return Ok(await _labResultService.GetByMedicalRecord(medicalRecordId));
        }
    }
}
