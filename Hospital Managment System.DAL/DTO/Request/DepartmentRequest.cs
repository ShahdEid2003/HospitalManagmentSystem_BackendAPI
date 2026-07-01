using Hospital_Managment_System.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class DepartmentRequest
    {
        public List<DepartmentTranslationsRequest> Translations { get; set; }
    }
}
