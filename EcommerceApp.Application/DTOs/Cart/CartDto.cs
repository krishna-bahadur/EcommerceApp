using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Decimal TotalPrice { get; set; }
        public List<CartItemDto> CartItemDto { get; set; } = new List<CartItemDto>();
    }
}
