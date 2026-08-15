using Hospital_Managment_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class BillResponse
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatusEnum PaymentStatus { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime IssuedAt { get; set; }

        public string? StripeUrl { get; set; }
    }
}
