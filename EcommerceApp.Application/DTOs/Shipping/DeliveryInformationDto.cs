using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Shipping
{
    public class DeliveryInformationDto
    {
        public string FullName { get; set; }
        public string Region { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string BuildingHouseNo { get; set; }
        public string? AddressLine { get; set; }
    }
}
