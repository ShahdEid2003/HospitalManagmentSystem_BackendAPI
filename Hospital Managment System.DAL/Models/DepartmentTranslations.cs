using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class DepartmentTranslations
    {
        public int Id { get; set; }
        public string Language { get; set; } = "en";
        public string Name { get; set; }

        public string? Description { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
