using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class AppointmentRequest
    {
        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public List<AppointmentTranslationRequest> Translations { get; set; }
            = new();
    }
}
