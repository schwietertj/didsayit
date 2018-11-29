using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DidSayItModels.Identity;
using Microsoft.AspNetCore.Identity;

namespace DidSayIt.Data
{
    public static class DbSeeder
    {
        public static async Task<bool> SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await SeedRoles(roleManager))
            {
                return await SeedUsers(userManager);
            }

            return false;
        }

        private static async Task<bool> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var email = AppSettings.Get<string>("adminEmail", true);
            var password = AppSettings.Get<string>("adminPassword", true);

            if (await userManager.FindByEmailAsync(email) is null)
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        UserName = email,
                        NormalizedUserName = email.ToUpper()
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, "Administrator");
                        if (!roleResult.Succeeded)
                        {
                            Console.WriteLine($"Could not add admin user to Administrator role. {string.Join(Environment.NewLine, roleResult.Errors.Select(x => x.Description).ToList())}");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Could not create admin user. {string.Join(Environment.NewLine, result.Errors.Select(x => x.Description).ToList())}");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not create admin user. {e.Message}");
                    return false;
                }
            }

            Console.WriteLine("Added admin user");
            return true;
        }

        private static async Task<bool> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> {"Administrator", "User"};

            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    try
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = r, NormalizedName = r.ToUpper() });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Could not create {r} role. {e.Message}");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
