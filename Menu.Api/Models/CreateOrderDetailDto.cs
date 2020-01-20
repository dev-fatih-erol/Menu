using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateOrderDetailDto
    {
        public int ProductId { get; set; }

        [Range(1, 50, ErrorMessage = "Lütfen adınızı girin")]
        [RegularExpression("^[1-9][0-9]?$|^100$", ErrorMessage = "Lütfen adınızı girin")]
        public byte Quantity { get; set; }

        public int[] OptionItems { get; set; }
    }
}