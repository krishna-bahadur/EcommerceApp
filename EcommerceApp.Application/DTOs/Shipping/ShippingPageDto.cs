using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Shipping
{
    public class ShippingPageDto
    {
        public DeliveryInformationDto DeliveryInformationDto { get; set; } = new();
        public OrderSummeryDto OrderSummeryDto { get; set; } = new();
    }
}
