using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class DoctorRatingSummaryResponse
    {
        public int DoctorId { get; set; }

        public double AverageRating { get; set; }

        public int TotalRatings { get; set; }
    }
}
