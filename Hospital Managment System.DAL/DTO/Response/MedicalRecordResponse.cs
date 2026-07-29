using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class MedicalRecordResponse
    {
        public int Id { get; set; }

        public string DoctorName { get; set; }

        public string PatientName { get; set; }

        public DateOnly VisitDate { get; set; }

        public string Diagnosis { get; set; }

        public string? Notes { get; set; }

        public string UserCreated { get; set; }
    }
}
