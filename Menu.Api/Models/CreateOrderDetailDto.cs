using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateOrderDetailDto
    {
        [Required(ErrorMessage = "Lütfen ürün seçin")]
        public int ProductId { get; set; }

        [Range(1, 50, ErrorMessage = "Ürün adedi 1 ile 50 arasında olmalıdır")]
        public byte Quantity { get; set; }

        public List<OrderDetailOptionDto> Options { get; set; }

        public int[] OptionItems { get; set; }
    }
}