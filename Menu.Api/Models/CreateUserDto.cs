using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Lütfen adınızı girin")]
        [MaxLength(30, ErrorMessage = "Lütfen geçerli bir ad girin")]
        [RegularExpression(@"^[a-zA-ZçıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir ad girin")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen soyadınızı girin")]
        [MaxLength(30, ErrorMessage = "Lütfen geçerli bir soyad girin")]
        [RegularExpression(@"^[a-zA-ZçıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir soyad girin")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Lütfen telefon numaranızı girin")]
        [RegularExpression(@"(^\+?[0-9]{10})$", ErrorMessage = "Lütfen geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır")]
        [RegularExpression(@"^[a-zA-Z0-9çıüğöşİĞÜÖŞÇ]+$", ErrorMessage = "Lütfen geçerli bir şifre girin")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen şehir seçin")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Lütfen sms kodunuzu girin")]
        public string Code { get; set; }

        public string Token { get; set; }
    }
}