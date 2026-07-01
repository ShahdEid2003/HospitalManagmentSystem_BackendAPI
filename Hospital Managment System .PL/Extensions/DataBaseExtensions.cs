using Hospital_Managment_System.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Managment_System_.PL.Extensions
{
    public static class DataBaseExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                 );
            return Services;
        }
    }
}
