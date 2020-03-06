using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu.Business.Extensions;
using Menu.Business.Models.CategoryViewModels;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Business.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        [Route("Category")]
        public IActionResult Index()
        {
            var model = _categoryService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
            });

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("Category/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("Category/Create")]
        public IActionResult Create(CreateViewModel model)
        {
            var categories = _categoryService.GetByVenueId(User.Identity.GetVenueId());

            var displayOrder = categories.OrderByDescending(x => x.DisplayOrder).FirstOrDefault().DisplayOrder;

            var newCategory = new Category
            {
                Name = model.Name.Trim(),
                DisplayOrder = displayOrder + 1,
                CreatedDate = DateTime.Now,
                VenueId = User.Identity.GetVenueId()
            };

            _categoryService.Create(newCategory);

            _categoryService.SaveChanges();

            return RedirectToAction("Index", "Category");
        }
    }
}