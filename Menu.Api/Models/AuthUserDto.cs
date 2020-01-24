using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class AuthUserDto
    {
        [Required(ErrorMessage = "Lütfen telefon numaranızı girin")]
        [RegularExpression(@"(^\+?[0-9]{10})$", ErrorMessage = "Lütfen geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }
    }
}