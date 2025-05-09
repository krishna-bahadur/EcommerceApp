using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Interfaces
{
    public interface IProductService
    {
        Task<Result<string>> CreateProductAsync(CreateProductDto createProductDto);
        Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync(int number, Guid? categoryId = null);
        Task<Result<string>> DeleteProductAsync(Guid id);
        Task<Result<UpdateProductDto>> GetProductForEditByIdAsync(Guid id);
        Task<Result<ProductDto>> GetProductBySlugAsync(string slug);
        Task<Result<string>> UpdateProductAsync(UpdateProductDto dto);
    }
}
