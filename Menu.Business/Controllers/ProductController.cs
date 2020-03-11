using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Menu.Business.Extensions;
using Menu.Business.Models.ProductViewModels;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Menu.Business.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;

        private readonly IProductService _productService;

        public ProductController(ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        [Route("Product/Create")]
        public IActionResult Create()
        {
            var categories = _categoryService.GetByVenueId(User.Identity.GetVenueId());

            if (categories == null || categories.Count() == 0)
            {
                return RedirectToAction("Index", "Category");
            }


            var model = new CreateViewModel
            {
                Code = "SG-" + DateTime.Now.ToString("yyMMddHHmmssff"),
                Categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Route("Product/Create")]
        public IActionResult Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            string photo;
            if (model.Photo != null)
            {
                var uniqueFileName = GetUniqueFileName(model.Photo.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                photo = uniqueFileName;
            }
            else
            {
                photo = "http://placehold.it/240x240";
            }

            var product = new Product
            {
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CreatedDate = DateTime.Now,
                CategoryId = model.Category,
                Photo = photo,
                DisplayOrder = 0,
                IsActive = false,
                OpeningTime = (model.Radio == "0") ? 0 : TimeSpan.Parse(model.OpeningTime).Ticks,
                ClosingTime = (model.Radio == "0") ? 864000000000 : TimeSpan.Parse(model.ClosingTime).Ticks
            };

            _productService.Create(product);

            _productService.SaveChanges();
           
            return RedirectToAction("Create");
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}