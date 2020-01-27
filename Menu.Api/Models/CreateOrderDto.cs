using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateOrderDto
    {
        public string Description { get; set; }

        [Required(ErrorMessage = "Lütfen sipariş detaylarını girin")]
        public List<CreateOrderDetailDto> OrderDetail { get; set; }
    }
}