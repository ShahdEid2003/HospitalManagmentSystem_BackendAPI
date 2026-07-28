using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class AppointmentResponse
    {
        public int Id { get; set; }

        public string DoctorName { get; set; }

        public string PatientName { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Status { get; set; }

        public string? Notes { get; set; }

        public string UserCreated { get; set; }
    }
}
