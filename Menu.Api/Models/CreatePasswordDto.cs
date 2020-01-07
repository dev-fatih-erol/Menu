using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreatePasswordDto
    {
        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Girmiş olduğunuz şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}