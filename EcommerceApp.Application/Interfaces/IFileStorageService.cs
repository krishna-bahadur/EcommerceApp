using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
