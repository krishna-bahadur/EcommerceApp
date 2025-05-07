using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.Common.Utilities;
using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace EcommerceApp.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProductService(IProductRepository productRepository, IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<string>> CreateProductAsync(CreateProductDto createProductDto)
        {
            string slug = StringUtilities.GenerateSlug(createProductDto.Name!);

            //Ensure slug uniqueness
            int attempt = 1;
            string originalSlug = slug;
            while(await _productRepository.ExistsBySlugAsync(slug))
            {
                slug = $"{originalSlug}-{attempt}";
                attempt++;
            }
            var productId = Guid.NewGuid();

            // Add Product Images
            List<ProductImage> images = new List<ProductImage>();

            foreach (var image in createProductDto.Images)
            {
                // Save to wwwroot folder
                var url = await _fileStorageService.SaveFileAsync(image);

                images.Add(new ProductImage(
                    Guid.NewGuid(),
                    url,
                    productId));
            }

            Product product = new Product(
                productId,
                createProductDto.Name,
                slug,
                createProductDto.Description,
                createProductDto.Price,
                createProductDto.DiscountPrice,
                createProductDto.StockQuantity,
                createProductDto.CategoryId,
                images);

            await _productRepository.AddAsync(product);

            return Result<string>.Success("Product added successfully.");
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllBlogsAsync(int number)
        {
            var products = await _productRepository.GetAllAsync();

            var productDtos = products.Select(MapToDto);

            return Result<IEnumerable<ProductDto>>.Success(productDtos);

        }

        

        public async Task<Result<string>> DeleteProductAsync(Guid id)
        {
            if (!await _productRepository.AnyAsync(id))
            {
                return Result<string>.Failure($"Product with Id {id} not found.");
            }

            await _productRepository.DeleteAsync(id);

            return Result<string>.Success("Product deleted successfully.");
        }

        public Task<Result<CreateProductDto>> UpdateProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product == null)
                return Result<ProductDto>.Failure($"Product with Id {id} not found.");

            var productDto = MapToDto(product);

            return Result<ProductDto>.Success(productDto);

        }




        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                StockQuantity = product.StockQuantity,
                Images = product.Images
            };
        }

    }
}
