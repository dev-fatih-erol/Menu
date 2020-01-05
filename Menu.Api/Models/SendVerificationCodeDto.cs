using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class SendVerificationCodeDto
    {
        [Required(ErrorMessage = "Lütfen telefon numaranızı girin")]
        [RegularExpression(@"(^\+?[0-9]{10})$", ErrorMessage = "Lütfen geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; }
    }
}