using System.Collections.Generic;

namespace Menu.Api.Models
{
    public class OrderDetailOptionDto
    {
        public int Id { get; set; }

        public List<OrderDetailOptionItemDto> OptionItems { get; set; }
    }

    public class OrderDetailOptionItemDto
    {
        public int Id { get; set; }
    }
}