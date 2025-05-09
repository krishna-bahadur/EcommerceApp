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
            while (await _productRepository.ExistsBySlugAsync(slug))
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

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync(int number, Guid? categoryId = null)
        {
            IEnumerable<Product> products;
            
            if(categoryId != null && categoryId.HasValue)
            {
                products = await _productRepository.GetByCategoryIdAsync((Guid) categoryId);
            }
            else
            {
                products = await _productRepository.GetAllAsync();
            }

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

        public async Task<Result<string>> UpdateProductAsync(UpdateProductDto dto)
        {
            try
            {

                var product = await _productRepository.GetAsync(dto.Id);

                if (product == null)
                {
                    return Result<string>.Failure($"Product with Id {dto.Id} not found");
                }

                // Check if Name is edited or not.
                string newSlug = product.Slug;
                if (product.Name!.Trim() != dto.Name!.Trim())
                {
                    newSlug = StringUtilities.GenerateSlug(dto.Name);

                    //Ensure slug uniqueness
                    int attempt = 1;
                    string originalSlug = newSlug;
                    while (await _productRepository.ExistsBySlugAsync(newSlug))
                    {
                        newSlug = $"{originalSlug}-{attempt}";
                        attempt++;
                    }
                }

                // Handle photo update case
                if (dto.RemovedImages.Any())
                {
                    foreach (var removedImage in dto.RemovedImages)
                    {
                        dto.ExistingImages.Remove(removedImage);
                    }
                }

                // Remove all existing product photos
                await _productRepository.DeleteProductImagesByProductId(dto.Id);

                // Add New and existing product photos again
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (var imageUrl in dto.ExistingImages)
                {
                    productImages.Add(new ProductImage(
                    Guid.NewGuid(),
                    imageUrl,
                    dto.Id
                    ));
                }

                //Check if new Images are uploaded again
                if (dto.Images?.Any() ?? false)
                {
                    foreach (var image in dto.Images)
                    {
                        // Save to wwwroot folder
                        var url = await _fileStorageService.SaveFileAsync(image);

                        productImages.Add(new ProductImage(
                            Guid.NewGuid(),
                            url,
                            dto.Id));
                    }
                }

                // Add product Images
                await _productRepository.AddProductImagesByProductId(productImages);

                product.Name = dto.Name;
                product.CategoryId = (Guid)dto.CategoryId;
                product.Price = (Decimal)dto.Price;
                product.DiscountPrice = dto.DiscountPrice;
                product.StockQuantity = (int)dto.StockQuantity;
                product.Description = dto.Description;
                product.Slug = newSlug;

                await _productRepository.UpdateAsync(product);

                return Result<string>.Success("Product updated successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<ProductDto>> GetProductBySlugAsync(string slug)
        {
            var product = await _productRepository.GetAsync(null, slug);

            if (product == null)
                return Result<ProductDto>.Failure($"Product with slug {slug} not found.");

            var productDtos = MapToDto(product);

            return Result<ProductDto>.Success(productDtos);
        }
            
            public async Task<Result<UpdateProductDto>> GetProductForEditByIdAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product == null)
                return Result<UpdateProductDto>.Failure($"Product with Id {id} not found.");

            UpdateProductDto updateProductDto = new UpdateProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                ExistingImages = product.Images.Select(x => x.ImageUrl).ToList()!
            };

            return Result<UpdateProductDto>.Success(updateProductDto);

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
                Images = product.Images,
                ImageUrls = product.Images.Select(x => x.ImageUrl).ToList()!,
            };
        }

    }
}
