using Blog.Application.Enums;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Persistance.Context
{
    public static class SeedData
    {
        public static async Task SeedEndpointsAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserRole>>();

            if (await dbContext.Set<Endpoint>().AnyAsync())
                return;

            var authorRole = await GetOrCreateRoleAsync(roleManager, UserRole.Author);
            var readerRole = await GetOrCreateRoleAsync(roleManager, UserRole.Reader);
            var adminRole = authorRole;

            // Create endpoints
            var endpoints = new List<Endpoint>
            {
                // BlogPost endpoints
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "POST",
                    Definition = "Create a blogpost",
                    Code = "POST.Writing.Createablogpost",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Reading.ToString(),
                    HttpType = "GET",
                    Definition = "Get all blogposts",
                    Code = "GET.Reading.Getallblogposts",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Reading.ToString(),
                    HttpType = "GET",
                    Definition = "Get blogpost by id",
                    Code = "GET.Reading.Getblogpostbyid",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Updating.ToString(),
                    HttpType = "PUT",
                    Definition = "Update blogpost",
                    Code = "PUT.Updating.Updateblogpost",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole }
                },

                // Comment endpoints
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "POST",
                    Definition = "Post a comment",
                    Code = "POST.Writing.Postacomment",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Reading.ToString(),
                    HttpType = "GET",
                    Definition = "Get all comments",
                    Code = "GET.Reading.Getallcomments",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Reading.ToString(),
                    HttpType = "GET",
                    Definition = "Get comments of a blogpost",
                    Code = "GET.Reading.Getcommentsofablogpost",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Deleting.ToString(),
                    HttpType = "DELETE",
                    Definition = "Delete comment",
                    Code = "DELETE.Deleting.Deletecomment",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { authorRole }
                },

                // User endpoints
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "PUT",
                    Definition = "Update author",
                    Code = "PUT.Writing.Updateauthor",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "GET",
                    Definition = "Get one user by its Id",
                    Code = "GET.Writing.GetoneuserbyitsId",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "POST",
                    Definition = "Update password",
                    Code = "POST.Writing.Updatepassword",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole, readerRole }
                },

                // Auth endpoints - typically not restricted, but included for completeness
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "POST",
                    Definition = "Login",
                    Code = "POST.Writing.Login",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole, readerRole }
                },
                new Endpoint
                {
                    Id = Guid.NewGuid(),
                    ActionType = ActionType.Writing.ToString(),
                    HttpType = "POST",
                    Definition = "Register",
                    Code = "POST.Writing.Register",
                    DateCreated = DateTime.UtcNow,
                    Roles = new List<ApplicationUserRole> { adminRole, authorRole, readerRole }
                }
            };

            await dbContext.AddRangeAsync(endpoints);
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserRole>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // Check if admin user already exists
            const string adminUsername = "admin";
            const string adminEmail = "admin@gmail.com";

            if (await userManager.FindByNameAsync(adminUsername) != null)
            {
                // Admin user already exists
                return;
            }

            // Get admin password from user secrets
            string adminPassword = configuration["AdminCredentials:Password"] ?? ""; // Default fallback if not in secrets

            // Create or get the Author role which will be used as admin role
            var authorRole = await GetOrCreateRoleAsync(roleManager, UserRole.Author);

            // Create application user first (Identity user) so we can get the hashed password
            var appUser = new ApplicationUser
            {
                UserName = adminUsername,
                Email = adminEmail
            };

            // Use Identity's password hashing
            var result = await userManager.CreateAsync(appUser, adminPassword);
            
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Get the hashed password from Identity
            var identityUser = await userManager.FindByNameAsync(adminUsername);
            string passwordHash = identityUser?.PasswordHash ?? string.Empty;

            // Create author instead of domain user
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                Name = adminUsername,
                PasswordHash = passwordHash, // Secure hash from Identity
                DateCreated = DateTime.UtcNow,
                ApplicationUserRoleId = authorRole.Id,
                About = "Administrator of the blog"
            };

            await dbContext.Authors.AddAsync(author);
            await dbContext.SaveChangesAsync();

            // Add to Author role
            await userManager.AddToRoleAsync(appUser, UserRole.Author.ToString());

            // Link the author back to the app user
            author.ApplicationUserId = appUser.Id;
            dbContext.Authors.Update(author);
            
            // Link the app user to the author
            appUser.DomainUserId = author.Id;
            await userManager.UpdateAsync(appUser);
            
            await dbContext.SaveChangesAsync();
        }

        private static async Task<ApplicationUserRole> GetOrCreateRoleAsync(
            RoleManager<ApplicationUserRole> roleManager, 
            UserRole roleType)
        {
            var roleName = roleType.ToString();
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                role = new ApplicationUserRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    RoleType = roleType
                    // Do not set DomainUserId here, as it causes FK constraint issues
                };
                await roleManager.CreateAsync(role);
            }

            return role;
        }
    }
} 