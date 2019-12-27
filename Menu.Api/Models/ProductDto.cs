using System;

namespace Menu.Api.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public bool IsAvailable { get; set; }
    }
}