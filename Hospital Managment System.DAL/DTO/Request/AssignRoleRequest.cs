using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.DTO.Request
{
    public class AssignRoleRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
