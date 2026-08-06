using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Response
{
    public class LabResultResponse
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public DateOnly ResultDate { get; set; }

        public string TestName { get; set; }

        public string Result { get; set; }

        public string? Notes { get; set; }

        public string UserCreated { get; set; }
    }
}
