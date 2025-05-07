using EcommerceApp.Domain.Constants.Roles;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password = "")
        {
            IdentityResult result;

            if (!string.IsNullOrEmpty(password))
                result = await _userManager.CreateAsync(user, password);
            else
                result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return result;

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
                if (!roleResult.Succeeded)
                    return roleResult;
            }

            return result;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<string>?> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user) as List<string>;
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, bool lockoutOnFailure = false)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure);
            return result;
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent = false)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

