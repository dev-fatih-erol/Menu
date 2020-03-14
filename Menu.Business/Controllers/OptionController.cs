using System;
using System.Collections.Generic;
using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.OptionViewModels;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class OptionController : Controller
    {
        private readonly IOptionService _optionService;

        private readonly IOptionItemService _optionItemService;

        private readonly IProductService _productService;

        public OptionController(IOptionService optionService,
            IOptionItemService optionItemService,
            IProductService productService)
        {
            _optionService = optionService;
            _optionItemService = optionItemService;
            _productService = productService;
        }

        [HttpPost]
        [Route("Product/{id:int}/Option")]
        public IActionResult Delete(int id, int optionId)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                var option = _optionService.GetByIdWithOptionItem(optionId);

                if (option is null)
                {
                    return NotFound();
                }

                _optionService.Delete(option);
                _optionService.SaveChanges();

                return RedirectToAction("Delete", "Option");
            }

            return NotFound();
        }

        [HttpGet]
        [Route("Product/{id:int}/Option")]
        public IActionResult Index(int id)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                ViewBag.Name = product.Name;

                var options = _optionService.GetByProductId(id);

                var model = options.Select(x => new IndexViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    OptionType = x.OptionType.ToOptionType(),
                    OptionItems = x.OptionItem.Select(y => new Models.OptionViewModels.OptionItem
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Price = y.Price
                    }).ToList()
                });

                return View(model);
            }

            return NotFound();
        }


        [HttpGet]
        [Route("Product/{id:int}/Option/Create")]
        public IActionResult Create(int id)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                ViewBag.Name = product.Name;

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

            return NotFound();
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
                    Title = model.Title,
                    CreatedDate = DateTime.Now,
                    OptionType = (OptionType)Enum.Parse(typeof(OptionType), model.OptionType)
                };

                _optionService.Create(newOption);

                _optionService.SaveChanges();

                foreach (var optionItem in model.OptionItems)
                {
                    var newOptionItem = new Core.Models.OptionItem
                    {
                        Name = optionItem.Name,
                        Price = optionItem.Price,
                        CreatedDate = DateTime.Now,
                        OptionId = newOption.Id
                    };

                    _optionItemService.Create(newOptionItem);
                    _optionItemService.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Option", new { id = product.Id });
        }
    }
}