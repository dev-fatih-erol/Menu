using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.CategoryViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Kategori adı boş olamaz")]
        public string Name { get; set; }
    }
}