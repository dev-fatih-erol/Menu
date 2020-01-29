using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateOrderDto
    {
        [StringLength(maximumLength: 250, ErrorMessage = "Açıklama, maximum 250 karakter uzunluğunda olmalıdır")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Lütfen sipariş detaylarını girin")]
        public List<CreateOrderDetailDto> OrderDetail { get; set; }
    }
}