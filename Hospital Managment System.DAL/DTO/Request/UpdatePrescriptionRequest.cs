using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class UpdatePrescriptionRequest
    {
        public int Id { get; set; }

        public List<PrescriptionTranslationsRequest> Translations { get; set; }
    }
}
