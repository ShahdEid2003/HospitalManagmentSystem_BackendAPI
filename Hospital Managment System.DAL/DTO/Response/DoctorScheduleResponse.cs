using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class DoctorScheduleResponse
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public string UserCreated { get; set; }
    }
}
