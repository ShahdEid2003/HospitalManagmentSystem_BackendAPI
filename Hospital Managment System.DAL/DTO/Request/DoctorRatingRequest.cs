using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class DoctorRatingRequest
    {
        public int DoctorId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
