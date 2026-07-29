using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class UpdateMedicalRecordRequest
    {
        public int Id { get; set; }

        public DateOnly VisitDate { get; set; }

        public List<MedicalRecordTranslationsRequest> Translations { get; set; }
            = new();
    }
}
