using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class AppointmentTranslationRequest
    {
        public int? Id { get; set; }
        public string Language { get; set; } = "en";

        public string? Notes { get; set; }
    }
}
