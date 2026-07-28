using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.AppointmentServices;
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
    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentService _appointmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AppointmentController(
            IAppointmentService appointmentService,
            IStringLocalizer<SharedResources> localizer)
        {
            _appointmentService = appointmentService;
            _localizer = localizer;
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create(AppointmentRequest request)
        {
            var result = await _appointmentService.Create(request);

            return Ok(result);
        }
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update(UpdateAppointmentRequest request)
        {
            var result = await _appointmentService.Update(request);

            return Ok(result);
        }

        [Authorize(Roles = "Receptionist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentService.Delete(id);

            return Ok(result);
        }

        [Authorize(Roles = "Receptionist,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _appointmentService.GetAll());
        }

        [Authorize(Roles = "Receptionist,Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _appointmentService.GetById(id));
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("Today")]
        public async Task<IActionResult> TodayAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _appointmentService.GetTodayAppointments(userId);

            return Ok(result);
        }
    }
}
