using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class DoctorRating : AuditableEntity
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        // Rating from 1 to 5
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public Doctor Doctor { get; set; }

        public Patient Patient { get; set; }
    }
}
