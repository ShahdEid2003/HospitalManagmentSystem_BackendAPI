using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class Prescription : AuditableEntity
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public List<PrescriptionTranslations> Translations { get; set; }
            
    }
}
