using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Auth;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<Result<AuthResponse>> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Result<AuthResponse>.Failure("Invalid login attempt");
            }

            var result = await _userRepository.PasswordSignInAsync(
                user.Email,
                model.Password,
                model.RememberMe
                );

            if (result.Succeeded)
            {
                var roles = await _userRepository.GetRolesAsync(user);
                return Result<AuthResponse>.Success(new AuthResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles ?? new List<string>()
                });
            }

            return Result<AuthResponse>.Failure("Invalid login attempt");
        }

        public async Task LogoutAsync()
        {
            await _userRepository.SignOutAsync();
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow
            };

            var result = await _userRepository.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return Result<AuthResponse>.Failure(result.Errors.Select(e => e.Description).FirstOrDefault()!.ToString());
            }

            await _userRepository.SignInAsync(user);

            return Result<AuthResponse>.Success(new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = new List<string>()
            });
        }


    }
}

