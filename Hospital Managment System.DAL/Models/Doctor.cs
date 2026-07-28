using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class Doctor:AuditableEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int DepartmentId { get; set; }

        public string Specialty { get; set; }

        public string LicenseNumber { get; set; }

        public ApplicationUser User { get; set; }
        public List<Appointment> Appointments { get; set; }= new List<Appointment>();

        //public Department Department { get; set; }

        //public  List<DoctorSchedule> DoctorSchedules { get; set; }

        //public  List<MedicalRecord> MedicalRecords { get; set; }

        //public  List<DoctorRating> DoctorRatings { get; set; }
    }
}
