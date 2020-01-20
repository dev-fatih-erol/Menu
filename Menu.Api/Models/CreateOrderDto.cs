using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateOrderDto
    {
        public string Description { get; set; }

        public int TableId { get; set; }

        public int VenueId { get; set; }

        [Required(ErrorMessage = "Lütfen adınızı girin")]
        public List<CreateOrderDetailDto> OrderDetail { get; set; }
    }
}