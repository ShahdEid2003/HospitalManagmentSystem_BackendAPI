using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Mangament_System.BLL.Services.FileService
{
    public interface IFileService
    {
        Task<string?> UploadeAsync(IFormFile file);
        void Delete(string fileName);
    }
}
