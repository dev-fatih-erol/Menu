using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.WaiterViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Lütfen garson adını girin")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen garson soyadını girin")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Lütfen garson kullanıcı adını girin")]
        [RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Lütfen geçerli bir kullanıcı adı girin")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lütfen garson şifresini girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }

        public TableViewModel[] TableViewModels { get; set; }
    }

    public class TableViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}