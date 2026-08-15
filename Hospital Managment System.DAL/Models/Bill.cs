using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public enum PaymentStatusEnum
    {
        Pending = 0,
        Paid = 1,
        Failed = 2
    }
    public enum PaymentMethodEnum
    {
        Cash = 0,
        Visa = 1
    }
    public class Bill
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatusEnum PaymentStatus { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime IssuedAt { get; set; }

        public string? StripeSessionId { get; set; }


        public Patient Patient { get; set; }

        public Appointment Appointment { get; set; }
    }
}
