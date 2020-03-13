using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.CashViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Kasa adı boş olamaz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen kullanıcı adınızı girin")]
        [RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Lütfen geçerli bir kullanıcı adı girin")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }
    }
}