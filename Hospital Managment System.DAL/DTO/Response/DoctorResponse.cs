using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class DoctorResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Specialty { get; set; }

        public string LicenseNumber { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}
