using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class Department : AuditableEntity
    {
        public int Id { get; set; }

        public string? ImageUrl { get; set; }

        public List<Doctor> Doctors { get; set; }
        public List<DepartmentTranslations> Translations { get; set; }
    }
}
