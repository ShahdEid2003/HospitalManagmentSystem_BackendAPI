using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System_.PL.Resources;
using Hospital_Mangament_System.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;
        private readonly IDepartmentService _IDepartmentService;
        public DepartmentsController(IDepartmentService IDepartmentService, IStringLocalizer<SharedResources> localizer)
        {
            _IDepartmentService = IDepartmentService;
            _localizer = localizer;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(DepartmentRequest request)
        {
            var response = await _IDepartmentService.CreateDepartment(request);


            return
                Ok(new
                {
                    response,
                    message = _localizer["Success"].Value
                });


        }
    }
}
