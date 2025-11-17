using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var hasUsers = userManager.Users.Any();
                var hasRoles = roleManager.Roles.Any();

                // Already seeded
                if (hasUsers && hasRoles)
                    return false;

                // Seed roles
                if (!hasRoles)
                {
                    var roles = new List<string> { "SuperAdmin", "Admin" };

                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role).Result)
                        {
                            roleManager.CreateAsync(new IdentityRole(role)).Wait();
                        }
                    }
                }

                // Seed users
                if (!hasUsers)
                {
                    var mainAdmin = new ApplicationUser
                    {
                        FirstName = "Nour Eldeen",
                        LastName = "Hegab",
                        UserName = "nour.eldeen",
                        Email = "Nour123@gmail.com",
                        PhoneNumber = "01023432343",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();

                    var admin = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Hegab",
                        UserName = "ahmed.hegab",
                        Email = "Ahmed123@gmail.com",
                        PhoneNumber = "01023432443",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    userManager.CreateAsync(admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Seed Data For Users : {ex}");
                return false;
            }
        }

    }
}
