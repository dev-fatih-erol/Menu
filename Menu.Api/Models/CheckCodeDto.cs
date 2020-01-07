using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CheckCodeDto
    {
        [Required(ErrorMessage = "Lütfen telefon numaranızı girin")]
        [RegularExpression(@"(^\+?[0-9]{10})$", ErrorMessage = "Lütfen geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Lütfen sms kodunuzu girin")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Lütfen güvenlik anahtarını girin")]
        public string Token { get; set; }
    }
}