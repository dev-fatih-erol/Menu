using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class AuthWaiterDto
    {
        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Lütfen geçerli bir kullanıcı adı girin")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }
    }
}