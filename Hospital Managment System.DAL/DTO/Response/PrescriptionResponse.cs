using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class PrescriptionResponse
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public string MedicationName { get; set; }

        public string Dosage { get; set; }

        public string Instructions { get; set; }

        public string UserCreated { get; set; }
    }
}
