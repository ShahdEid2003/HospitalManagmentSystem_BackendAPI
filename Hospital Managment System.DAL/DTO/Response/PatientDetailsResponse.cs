using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class PatientDetailsResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string NationalId { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string MedicalRecordNumber { get; set; }
    }
}
