using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class RegisterRequest
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        // Patient
        public string? NationalId { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

        // Doctor
        public int? DepartmentId { get; set; }
        public string? Specialty { get; set; }
        public string? LicenseNumber { get; set; }
    }
}
