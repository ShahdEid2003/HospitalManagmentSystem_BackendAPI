using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Mangament_System.BLL.Services.DoctorScheduleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _doctorScheduleService;

        public DoctorScheduleController(IDoctorScheduleService doctorScheduleService)
        {
            _doctorScheduleService = doctorScheduleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorScheduleRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result =await _doctorScheduleService.Create(request, userId);

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateDoctorScheduleRequest request)
        {
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result =await _doctorScheduleService.Update(request, userId);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _doctorScheduleService.Delete(id, userId);

            return Ok(result);
        }

        [HttpGet("MySchedule")]
        public async Task<IActionResult> GetMySchedule()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _doctorScheduleService.GetMySchedule(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _doctorScheduleService.GetById(id);

            return Ok(result);
        }
    }
}
