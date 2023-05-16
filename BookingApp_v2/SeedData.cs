using BookingApp_v2.Data;
using Microsoft.AspNetCore.Identity;

namespace BookingApp_v2
{
    public static class SeedData
    {
        public static void Seed(
            UserManager<Client> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<Client> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new Client
                {
                    UserName = "admin",
                    Email = "admin@localhost.com",
                };
                var result = userManager.CreateAsync(user, "P@ssword1").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Client").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Client"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
