using System.ComponentModel.DataAnnotations;

namespace Menu.Api.Models
{
    public class CreateCommentRatingDto
    {
        [MinLength(10, ErrorMessage = "Yorum en az 10 karakter uzunluğunda olmalıdır")]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "Puan oranı 1 ile 5 arasında olmalıdır")]
        public byte Speed { get; set; }

        [Range(1, 5, ErrorMessage = "Puan oranı 1 ile 5 arasında olmalıdır")]
        public byte Waiter { get; set; }

        [Range(1, 5, ErrorMessage = "Puan oranı 1 ile 5 arasında olmalıdır")]
        public byte Flavor { get; set; }

        public int OrderTableId { get; set; }
    }
}