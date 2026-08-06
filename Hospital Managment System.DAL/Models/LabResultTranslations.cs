using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Models
{
    public class LabResultTranslations
    {
        public int Id { get; set; }

        public string Language { get; set; } = "en";

        public string TestName { get; set; }

        public string Result { get; set; }

        public string? Notes { get; set; }

        public int LabResultId { get; set; }

        public LabResult LabResult { get; set; }
    }
}
