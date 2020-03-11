using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Menu.Business.Models.ProductViewModels
{
    public class CreateViewModel
    {
        public string Code { get; set; }

        [Required(ErrorMessage = "Lütfen ürün adını girin")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Lütfen geçerli uzunlukta ürün adı girin")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen ürün açıklaması girin")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "Lütfen geçerli uzunlukta ürün açıklaması girin")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Lütfen ürün fiyatı girin")]
        [Range(1.00, 999.99, ErrorMessage = "Lütfen geçerli ürün fiyatı girin")]
        [RegularExpression(@"^\d+\,\d{0,2}$", ErrorMessage = "Lütfen geçerli ürün fiyatı girin")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Lütfen ürün kategorisi seçin")]
        public int Category { get; set; }

        public List<SelectListItem> Categories { set; get; }

        public string OpeningTime { get; set; }

        public string ClosingTime { get; set; }

        public string Radio { get; set; }

        public IFormFile Photo { set; get; }
    }
}