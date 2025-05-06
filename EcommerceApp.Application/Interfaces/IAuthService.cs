using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginDto model);
        Task<Result<AuthResponse>> RegisterAsync(RegisterDto model);
        Task LogoutAsync();
    }
}
