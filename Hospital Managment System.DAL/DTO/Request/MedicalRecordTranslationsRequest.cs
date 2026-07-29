using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class MedicalRecordTranslationsRequest
    {
        public int? Id { get; set; }

        public string Language { get; set; } = "en";

        public string Diagnosis { get; set; }

        public string? Notes { get; set; }
    }
}
