using System;

namespace Menu.Core.Models
{
    public class AppAbout
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}