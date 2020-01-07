using System;
using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Lütfen eski şifrenizi girin")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Girmiş olduğunuz şifreler eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
