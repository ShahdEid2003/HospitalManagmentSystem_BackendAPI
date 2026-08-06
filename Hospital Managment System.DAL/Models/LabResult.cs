using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class LabResult : AuditableEntity
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public MedicalRecord MedicalRecord { get; set; }

        public DateOnly ResultDate { get; set; }

        public List<LabResultTranslations> Translations { get; set; } 
    }
}
