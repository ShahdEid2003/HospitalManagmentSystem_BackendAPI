using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class Patient : AuditableEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string NationalId { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string MedicalRecordNumber { get; set; }

        public ApplicationUser User { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
     

        public List<MedicalRecord> MedicalRecords { get; set; }

        //public  List<Bill> Bills { get; set; }

        public List<DoctorRating> DoctorRatings { get; set; }
    }
}
