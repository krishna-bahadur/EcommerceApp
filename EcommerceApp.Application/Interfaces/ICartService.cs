using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Cart;
using EcommerceApp.Domain.Entities;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICartService
    {
        Task<Result<CartDto>> GetOrCreateCartAsync(Guid userId);
        Task<Result<string>> AddToCartAsync(Guid userId, Guid productId, int quantity);
        Task<Result<string>> RemoveFromCartAsync(Guid userId, Guid productId);
        Task<Result<string>> UpdateCartItemQuantityAsync(Guid userId, Guid productId, int quantity);
        Task<Result<string>> ClearCartAsync(Guid userId);
    }
}
