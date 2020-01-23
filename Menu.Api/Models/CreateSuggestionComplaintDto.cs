using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateSuggestionComplaintDto
    {
        [Required(ErrorMessage = "Lütfen açıklama girin")]
        [MinLength(10, ErrorMessage = "Lütfen minimum 10 karakter girin")]
        public string Description { get; set; }

        public int SubjectType { get; set; }
    }
}