using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Mangament_System.BLL.Services.BillServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Managment_System_.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Receptionist,Admin")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }


    

        [HttpPost("checkout")]
        public async Task<IActionResult> ProcessBill([FromBody] BillRequest request)
        {
            var result = await _billService.ProcessBill(request);

            return Ok(result);
        }


      

        [HttpGet]
        public async Task<IActionResult> GetBills()
        {
            var result = await _billService.GetBills();

            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillById(int id)
        {
            var result = await _billService.GetBillById(id);

            return Ok(result);
        }


        // =====================================================
        // Get All Bills For Specific Patient
        // =====================================================

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientBills(
            int patientId)
        {
            var result =
                await _billService.GetPatientBills(patientId);

            return Ok(result);
        }


        // =====================================================
        // Stripe Success
        // =====================================================

        [AllowAnonymous]
        [HttpGet("success")]
        public async Task<IActionResult> Success(
            [FromQuery] string sessionId)
        {
            var result =
                await _billService.HandleSuccess(sessionId);

            return Ok(result);
        }


        // =====================================================
        // Stripe Cancel
        // =====================================================

        [AllowAnonymous]
        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return Ok(new
            {
                Success = false,
                Message = "Payment was cancelled."
            });
        }
    }
}
