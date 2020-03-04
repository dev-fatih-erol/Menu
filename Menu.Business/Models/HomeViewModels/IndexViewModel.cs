using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.HomeViewModels
{
    public class IndexViewModel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı girin")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "Lütfen geçerli kullanıcı adı girin")]
        [RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Lütfen geçerli kullanıcı adı girin")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Lütfen geçerli şifre girin")]
        [RegularExpression(@"^[A-Za-z0-9-_.]+$", ErrorMessage = "Lütfen geçerli şifre girin")]
        public string Password { get; set; }
    }
}