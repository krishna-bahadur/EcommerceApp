using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Category
{
    public class CategoryPageDto
    {
        public CreateCategoryDto CreateCategoryDto { get; set; }
        public List<CategoryDto> CategoryDtos { get; set; }

        public CategoryPageDto()
        {
            CreateCategoryDto = new CreateCategoryDto();
            CategoryDtos = new List<CategoryDto>();
        }
    }
}
