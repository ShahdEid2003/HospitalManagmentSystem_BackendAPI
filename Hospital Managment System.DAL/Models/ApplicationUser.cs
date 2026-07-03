using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public bool IsApproved { get; set; } = true;
        public string? CodeRestPassword { get; set; }
        public DateTime? PasswordRestCodeExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
