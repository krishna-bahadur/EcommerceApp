using EcommerceApp.Domain.Constants.Roles;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using EcommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceApp.Infrastructure.Seeds
{
    public static class DataSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Seed Roles
                    if (!await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
                    }

                    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                    }

                    // Seed SuperAdmin
                    var email = "superadmin@gmail.com";
                    var userResult = await _userManager.FindByEmailAsync(email);
                    if (userResult is null)
                    {
                        var password = "Admin@321";
                        ApplicationUser user = new()
                        {
                            FullName = "Superadmin",
                            UserName = "superadmin@gmail.com",
                            Email = email,
                            EmailConfirmed = true,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            CreatedOn= DateTime.UtcNow
                        };

                        var result = await _userManager.CreateAsync(user, password);

                        if (await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                        {
                            await _userManager.AddToRoleAsync(user, UserRoles.SuperAdmin);
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
