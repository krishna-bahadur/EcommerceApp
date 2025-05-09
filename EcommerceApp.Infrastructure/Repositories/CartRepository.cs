using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using EcommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetByIdAsync(Guid id)
        {
            return await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task UpdateAsync(Cart cart)
        {
            var existingDbCartItems = await _context.CartItems
                .Where(x => x.CartId == cart.Id)
                .ToListAsync();

            var newItems = cart.Items
                .Where(ci => !existingDbCartItems.Any(db => db.Id == ci.Id))
                .ToList();

            var updatedItems = cart.Items
                .Where(ci => existingDbCartItems.Any(db => db.Id == ci.Id))
                .ToList();

            // Attach updated items to the context
            foreach (var item in updatedItems)
            {
                _context.Entry(item).State = EntityState.Modified;
            }

            // Add new items
            await _context.CartItems.AddRangeAsync(newItems);

            // Update the cart itself (if needed)
            _context.Entry(cart).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
