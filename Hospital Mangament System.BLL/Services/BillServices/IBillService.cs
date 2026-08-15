using Hospital_Managment_System.DAL.DTO.Request;
using Hospital_Managment_System.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.BillServices
{
    public interface IBillService
    {
        Task<BillResponse> ProcessBill(BillRequest request);

        Task<BillResponse> HandleSuccess( string sessionId);

        Task<BillResponse> GetBillById(int billId);

        Task<List<BillResponse>> GetBills();

        Task<List<BillResponse>> GetPatientBills(int patientId);
    }
}
