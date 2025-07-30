using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { "SuperAdmin", "ProjectManager", "TeamLead", "Member" };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var superAdmin = await userManager.FindByEmailAsync("admin@taskmanagement.com");
            
            if (superAdmin == null)
            {
                superAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@taskmanagement.com",
                    FullName = "Super Administrator",
                    Role = UserRole.SuperAdmin,
                    IsActive = true,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                
                var result = await userManager.CreateAsync(superAdmin, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, UserRole.SuperAdmin.ToString());
                }
            }
        }
    }
} 