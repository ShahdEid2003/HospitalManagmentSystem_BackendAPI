using Hospital_Managment_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Managment_System.DAL.Utiles
{
    public class AdminSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DataSeed()
        {
            string email = "admin@hospital.com";
            string password = "Admin@123456";

            var admin = await _userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FullName = "System Admin",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    IsApproved = true
                };

                var result = await _userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(x => x.Description))
                    );
                }
            }

            if (!await _userManager.IsInRoleAsync(admin, "Admin"))
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
