using EcommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password = "");
        Task SignInAsync(ApplicationUser user, bool isPersistent = false);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, bool lockoutOnFailure = false);
        Task<List<string>?> GetRolesAsync(ApplicationUser user);
        Task SignOutAsync();
    }
}
