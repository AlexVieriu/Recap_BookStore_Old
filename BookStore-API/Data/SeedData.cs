using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager,
                                      RoleManager<IdentityRole> roleManager)
        {
            await SeedRole(roleManager);
            await SeedUser(userManager);
        }

        public async static Task SeedUser(UserManager<IdentityUser> userManager)
        {
            if(await userManager.FindByNameAsync("Admin") == null)
            {
                var user = new IdentityUser()
                {
                    UserName = "Admin",
                    Email = "admin@bookstore.com"
                };

                var userCreated = await userManager.CreateAsync(user, "P@ssword1");
                if(userCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,"Administrator");
                }
            }

            if(await userManager.FindByEmailAsync("Customer1") == null)
            {
                var user = new IdentityUser()
                {
                    UserName = "Customer1",
                    Email = "customer1@bookstore.com"
                };

                var userCreated = await userManager.CreateAsync(user, "P@ssword1");
                if(userCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }

            if(await userManager.FindByEmailAsync("Customer2") == null)
            {
                var user = new IdentityUser()
                {
                    UserName = "Customer2",
                    Email = "customer2@bookstore.com"
                };

                var userCreated = await userManager.CreateAsync(user, "P@ssword1");
                if(userCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,"Customer");
                }
            }
        }

        public async static Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync("Administrator") == false)
            {
                var role = new IdentityRole()
                {
                    Name = "Administrator"
                };

                await roleManager.CreateAsync(role);
            }

            if (await roleManager.RoleExistsAsync("Customer") == false)
            {
                var role = new IdentityRole()
                {
                    Name = "Customer"
                };

                await roleManager.CreateAsync(role);
            };
        }
    }
}
