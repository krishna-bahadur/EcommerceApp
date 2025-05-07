using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // Set the upload folder path (e.g., wwwroot/uploads)
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            // Unique filename to avoid conflicts
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path (e.g., /uploads/abc123.jpg)
            return $"/uploads/{uniqueFileName}";
        }
    }
}
