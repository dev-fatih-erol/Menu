using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu.Business.Models.OptionViewModels;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class OptionController : Controller
    {
        private readonly IOptionService _optionService;

        private readonly IProductService _productService;

        public OptionController(IOptionService optionService,
            IProductService productService)
        {
            _optionService = optionService;
            _productService = productService;
        }

        [HttpGet]
        [Route("Product/{id:int}/Option/Create")]
        public IActionResult Create()
        {

            var model = new CreateViewModel
            {
                OptionItems = new List<Models.OptionViewModels.OptionItem> {
                    new Models.OptionViewModels.OptionItem{
                        Name = string.Empty,
                        Price = 0
                    },
                    new Models.OptionViewModels.OptionItem{
                        Name = string.Empty,
                        Price = 0
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        [Route("Product/{id:int}/Option/Create")]
        public IActionResult Create(int id, CreateViewModel model)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                var newOption = new Option
                {
                    ProductId = product.Id,
                    Title = model.Title
                };

                _optionService.Create(newOption);
                _optionService.SaveChanges();

            }

            return View();
        }
    }
}