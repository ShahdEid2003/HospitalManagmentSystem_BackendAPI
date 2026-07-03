using Hospital_Managment_System.DAL.Data;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.GenericRepository;
using Hospital_Managment_System.DAL.Repository.PatientRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Repository.DoctorRepositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {


        }
    }
}
