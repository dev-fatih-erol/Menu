using System;

namespace Menu.Core.Models
{
    public class AppSlider
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public int DisplayOrder { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}