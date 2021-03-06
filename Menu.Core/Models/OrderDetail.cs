﻿namespace Menu.Core.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OptionItem { get; set; }

        public string Photo { get; set; }

        public byte Quantity { get; set; }

        public decimal Price { get; set; }


        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}