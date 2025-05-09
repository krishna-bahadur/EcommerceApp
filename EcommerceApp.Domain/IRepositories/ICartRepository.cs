using EcommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Domain.IRepositories
{
    public interface ICartRepository
    {
        Task AddAsync(Cart cart);
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(Cart cart);
    }
}
