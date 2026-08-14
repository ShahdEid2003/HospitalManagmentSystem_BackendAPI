using Hospital_Managment_System.DAL.Data;
using Hospital_Managment_System.DAL.Models;
using Hospital_Managment_System.DAL.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Repository.BookingRepositories
{
    public class AppointmentBookingRepository
    : GenericRepository<AppointmentBooking>,
      IAppointmentBookingRepository
    {
        public AppointmentBookingRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
