using System.Collections.Generic;

namespace Menu.Api.Models
{
    public class CategoryProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}