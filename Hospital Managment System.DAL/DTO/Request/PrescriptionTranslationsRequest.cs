using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class PrescriptionTranslationsRequest
    {
        public int? Id { get; set; }

        public string Language { get; set; } = "en";

        public string MedicationName { get; set; }

        public string Dosage { get; set; }

        public string Instructions { get; set; }
    }
}
