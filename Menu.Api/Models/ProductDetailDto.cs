using System;
using System.Collections.Generic;

namespace Menu.Api.Models
{
    public class ProductDetailDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public List<OptionOptionItemDto> Options { get; set; }
    }
}