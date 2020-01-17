using System.Collections.Generic;

namespace Menu.Api.Models
{
    public class CreateOrderDto
    {
        public string Description { get; set; }

        public int TableId { get; set; }

        public int VenueId { get; set; }

        public List<CreateOrderDetailDto> OrderDetail { get; set; }
    }
}