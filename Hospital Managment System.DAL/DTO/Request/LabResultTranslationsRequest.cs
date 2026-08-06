using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class LabResultTranslationsRequest
    {
        public int? Id { get; set; }

        public string Language { get; set; }

        public string TestName { get; set; }

        public string Result { get; set; }

        public string? Notes { get; set; }
    }
}
