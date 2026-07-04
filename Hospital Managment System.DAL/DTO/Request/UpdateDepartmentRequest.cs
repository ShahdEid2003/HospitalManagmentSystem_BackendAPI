using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class UpdateDepartmentRequest
    {
        public int Id { get; set; }
        public List<DepartmentTranslationsRequest> Translations { get; set; }
    }
}
