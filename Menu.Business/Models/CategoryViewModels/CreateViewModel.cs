using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.CategoryViewModels
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}