using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class LabResultRequest
    {
        public int MedicalRecordId { get; set; }

        public DateOnly ResultDate { get; set; }

        public List<LabResultTranslationsRequest> Translations { get; set; }
    }

    
}
