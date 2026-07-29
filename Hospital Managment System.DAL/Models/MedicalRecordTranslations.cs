using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class MedicalRecordTranslations
    {
        public int Id { get; set; }

        public string Language { get; set; } = "en";

        public string Diagnosis { get; set; }

        public string? Notes { get; set; }

        public int MedicalRecordId { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
    }
}
