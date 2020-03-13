using System.ComponentModel.DataAnnotations;

namespace Menu.Business.Models.TableViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Masa Adı boş olamaz")]
        public string Name { get; set; }
    }
}