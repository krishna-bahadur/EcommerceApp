using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Cart;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<CartDto>> GetOrCreateCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            
            if(cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            CartDto dto = new CartDto()
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.CalculateTotal()
            };

            // Add Cart items in dto
            foreach (var item in cart.Items)
            {
                dto.CartItemDto.Add(new CartItemDto()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.LineTotal
                });
            }

            return Result<CartDto>.Success(dto);
        }

        public async Task<Result<string>> AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            
            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            var product = await _productRepository.GetAsync(productId);

            if (product == null)
                return Result<string>.Failure($"Product with Id {productId} not found");

            cart.AddItem(product, quantity);
            await _cartRepository.UpdateAsync(cart);

            return Result<string>.Success("Item added to cart successfully!");
        }

        public async Task<Result<string>> ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            cart.Clear();
            await _cartRepository.UpdateAsync(cart);

            return Result<string>.Success("Cart has been Clear");

        }



        public async Task<Result<string>> RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);

            return Result<string>.Success("Cart Item Removed.");
        }

        public async Task<Result<string>> UpdateCartItemQuantityAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            cart.UpdateItemQuantity(productId, quantity);
            await _cartRepository.UpdateAsync(cart);

            return Result<string>.Success("Item quantity Updated.");
        }
    }
}
