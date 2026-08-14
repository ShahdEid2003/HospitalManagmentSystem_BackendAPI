using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class PatientMedicalRecordResponse
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public DateOnly VisitDate { get; set; }
        public string? Notes { get; set; }
    }
}
