using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class MedicalRecord : AuditableEntity
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public DateOnly VisitDate { get; set; }

        public List<MedicalRecordTranslations> Translations { get; set; }
          

        //public List<Prescription> Prescriptions { get; set; }
        //    = new();

        //public List<LabResult> LabResults { get; set; }
        //    = new();
    }
}
