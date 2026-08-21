using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;
        private readonly IDepartmentService _IDepartmentService;
        public DepartmentsController(IDepartmentService IDepartmentService, IStringLocalizer<SharedResources> localizer)
        {
            _IDepartmentService = IDepartmentService;
            _localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var departments = await _IDepartmentService.GetAll();

            return Ok(new { data = departments, _localizer["Success"].Value });

        }
       

        [HttpPost("")]
        public async Task<IActionResult> Create( [FromForm] DepartmentRequest request)
        {
            var response = await _IDepartmentService.CreateDepartment(request);

            return Ok(new
            {
                response,
                message = _localizer["Success"].Value
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _IDepartmentService.GetDepartment(b => b.Id == id);

            return Ok(new { data = department, _localizer["Success"].Value });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _IDepartmentService.Delete(id);
            if (!result) return NotFound(new { messege = _localizer["NotFound"].Value });
            return Ok(new { messege = _localizer["Success"].Value }); ;


        }
        [HttpPatch("")]
        public async Task<IActionResult> UpdateDepartment(
          [FromForm] UpdateDepartmentRequest request)
        {
            var result =
                await _IDepartmentService.Update(request);

            if (!result)
                return NotFound(new
                {
                    message = _localizer["NotFound"].Value
                });

            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }
    }
}
