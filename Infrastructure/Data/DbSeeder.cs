using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public static async Task SeedMembersAsync(UserManager<ApplicationUser> userManager)
        {
            var members = new[]
            {
                new { UserName = "john.doe", Email = "john.doe@company.com", FullName = "John Doe", Role = UserRole.ProjectManager, Password = "John123!" },
                new { UserName = "jane.smith", Email = "jane.smith@company.com", FullName = "Jane Smith", Role = UserRole.TeamLead, Password = "Jane123!" },
                new { UserName = "mike.wilson", Email = "mike.wilson@company.com", FullName = "Mike Wilson", Role = UserRole.Member, Password = "Mike123!" },
                new { UserName = "sarah.jones", Email = "sarah.jones@company.com", FullName = "Sarah Jones", Role = UserRole.Member, Password = "Sarah123!" },
                new { UserName = "david.brown", Email = "david.brown@company.com", FullName = "David Brown", Role = UserRole.Member, Password = "David123!" },
                new { UserName = "emma.davis", Email = "emma.davis@company.com", FullName = "Emma Davis", Role = UserRole.TeamLead, Password = "Emma123!" },
                new { UserName = "alex.taylor", Email = "alex.taylor@company.com", FullName = "Alex Taylor", Role = UserRole.Member, Password = "Alex123!" },
                new { UserName = "lisa.garcia", Email = "lisa.garcia@company.com", FullName = "Lisa Garcia", Role = UserRole.Member, Password = "Lisa123!" }
            };

            foreach (var member in members)
            {
                var existingUser = await userManager.FindByEmailAsync(member.Email);
                
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = member.UserName,
                        Email = member.Email,
                        FullName = member.FullName,
                        Role = member.Role,
                        IsActive = true,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    
                    var result = await userManager.CreateAsync(user, member.Password);
                    
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, member.Role.ToString());
                    }
                }
            }
        }

        public static async Task SeedProjectsAsync(AppDbContext context)
        {
            if (!context.Projects.Any())
            {
                var projects = new[]
                {
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "E-Commerce Platform Development",
                        Description = "Building a modern e-commerce platform with React frontend and .NET backend",
                        DueDate = DateTime.UtcNow.AddMonths(6),
                        CreatedAt = DateTime.UtcNow.AddDays(-30)
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mobile App for Task Management",
                        Description = "Developing a cross-platform mobile application for task and project management",
                        DueDate = DateTime.UtcNow.AddMonths(4),
                        CreatedAt = DateTime.UtcNow.AddDays(-20)
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Customer Relationship Management System",
                        Description = "Implementing a comprehensive CRM system with advanced analytics and reporting",
                        DueDate = DateTime.UtcNow.AddMonths(8),
                        CreatedAt = DateTime.UtcNow.AddDays(-15)
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Data Analytics Dashboard",
                        Description = "Creating interactive dashboards for business intelligence and data visualization",
                        DueDate = DateTime.UtcNow.AddMonths(3),
                        CreatedAt = DateTime.UtcNow.AddDays(-10)
                    },
                    new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "API Gateway and Microservices",
                        Description = "Building a scalable API gateway with microservices architecture",
                        DueDate = DateTime.UtcNow.AddMonths(5),
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    }
                };

                await context.Projects.AddRangeAsync(projects);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedProjectMembersAsync(AppDbContext context)
        {
            if (!context.ProjectMembers.Any())
            {
                var projects = await context.Projects.ToListAsync();
                var users = await context.Users.ToListAsync();

                if (projects.Any() && users.Any())
                {
                    var projectMembers = new List<ProjectMember>();

                    // E-Commerce Platform Development
                    var ecommerceProject = projects.FirstOrDefault(p => p.Name.Contains("E-Commerce"));
                    if (ecommerceProject != null)
                    {
                        var projectManager = users.FirstOrDefault(u => u.FullName == "John Doe");
                        var teamLead = users.FirstOrDefault(u => u.FullName == "Jane Smith");
                        var members = users.Where(u => u.FullName.Contains("Mike") || u.FullName.Contains("Sarah")).ToList();

                        if (projectManager != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = projectManager.Id,
                                ProjectId = ecommerceProject.Id,
                                Role = "ProjectManager",
                                Team = "Management"
                            });
                        }

                        if (teamLead != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = teamLead.Id,
                                ProjectId = ecommerceProject.Id,
                                Role = "TeamLead",
                                Team = "Frontend"
                            });
                        }

                        foreach (var member in members)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = member.Id,
                                ProjectId = ecommerceProject.Id,
                                Role = "Member",
                                Team = "Development"
                            });
                        }
                    }

                    // Mobile App for Task Management
                    var mobileProject = projects.FirstOrDefault(p => p.Name.Contains("Mobile App"));
                    if (mobileProject != null)
                    {
                        var teamLead = users.FirstOrDefault(u => u.FullName == "Emma Davis");
                        var members = users.Where(u => u.FullName.Contains("David") || u.FullName.Contains("Alex")).ToList();

                        if (teamLead != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = teamLead.Id,
                                ProjectId = mobileProject.Id,
                                Role = "TeamLead",
                                Team = "Mobile Development"
                            });
                        }

                        foreach (var member in members)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = member.Id,
                                ProjectId = mobileProject.Id,
                                Role = "Member",
                                Team = "Development"
                            });
                        }
                    }

                    // Customer Relationship Management System
                    var crmProject = projects.FirstOrDefault(p => p.Name.Contains("Customer Relationship"));
                    if (crmProject != null)
                    {
                        var projectManager = users.FirstOrDefault(u => u.FullName == "John Doe");
                        var teamLead = users.FirstOrDefault(u => u.FullName == "Jane Smith");
                        var members = users.Where(u => u.FullName.Contains("Lisa")).ToList();

                        if (projectManager != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = projectManager.Id,
                                ProjectId = crmProject.Id,
                                Role = "ProjectManager",
                                Team = "Management"
                            });
                        }

                        if (teamLead != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = teamLead.Id,
                                ProjectId = crmProject.Id,
                                Role = "TeamLead",
                                Team = "Backend"
                            });
                        }

                        foreach (var member in members)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = member.Id,
                                ProjectId = crmProject.Id,
                                Role = "Member",
                                Team = "Development"
                            });
                        }
                    }

                    // Data Analytics Dashboard
                    var analyticsProject = projects.FirstOrDefault(p => p.Name.Contains("Data Analytics"));
                    if (analyticsProject != null)
                    {
                        var teamLead = users.FirstOrDefault(u => u.FullName == "Emma Davis");
                        var members = users.Where(u => u.FullName.Contains("Mike") || u.FullName.Contains("Sarah")).ToList();

                        if (teamLead != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = teamLead.Id,
                                ProjectId = analyticsProject.Id,
                                Role = "TeamLead",
                                Team = "Data Science"
                            });
                        }

                        foreach (var member in members)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = member.Id,
                                ProjectId = analyticsProject.Id,
                                Role = "Member",
                                Team = "Analytics"
                            });
                        }
                    }

                    // API Gateway and Microservices
                    var apiProject = projects.FirstOrDefault(p => p.Name.Contains("API Gateway"));
                    if (apiProject != null)
                    {
                        var projectManager = users.FirstOrDefault(u => u.FullName == "John Doe");
                        var teamLead = users.FirstOrDefault(u => u.FullName == "Jane Smith");
                        var members = users.Where(u => u.FullName.Contains("David") || u.FullName.Contains("Alex")).ToList();

                        if (projectManager != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = projectManager.Id,
                                ProjectId = apiProject.Id,
                                Role = "ProjectManager",
                                Team = "Management"
                            });
                        }

                        if (teamLead != null)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = teamLead.Id,
                                ProjectId = apiProject.Id,
                                Role = "TeamLead",
                                Team = "Backend"
                            });
                        }

                        foreach (var member in members)
                        {
                            projectMembers.Add(new ProjectMember
                            {
                                UserId = member.Id,
                                ProjectId = apiProject.Id,
                                Role = "Member",
                                Team = "Development"
                            });
                        }
                    }

                    await context.ProjectMembers.AddRangeAsync(projectMembers);
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedAllAsync(RoleManager<IdentityRole<Guid>> roleManager, 
                                           UserManager<ApplicationUser> userManager, 
                                           AppDbContext context)
        {
            await SeedRolesAsync(roleManager);
            await SeedSuperAdminAsync(userManager);
            await SeedMembersAsync(userManager);
            await SeedProjectsAsync(context);
            await SeedProjectMembersAsync(context);
        }
    }
} 