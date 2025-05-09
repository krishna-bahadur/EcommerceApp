using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Shipping
{
    public class OrderSummeryDto
    {
        public int TotalItems { get; set; }
        public Decimal Price { get; set; }
        public Decimal DeliveryFee { get; set; }
        public Decimal TotalPrice { get; set; } 
    }
}
