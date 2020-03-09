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

        [HttpPost]
        [Authorize]
        [Route("Category/Edit/{id:int}")]
        public IActionResult Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.GetById(id);

                if (category.VenueId == User.Identity.GetVenueId())
                {
                    if (category != null)
                    {
                        category.Name = model.Name;

                        _categoryService.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Category/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);

            if (category != null)
            {
                if (category.VenueId == User.Identity.GetVenueId())
                {
                    var model = new EditViewModel()
                    {
                        Name = category.Name
                    };

                    return View(model);
                }
            }

            return NotFound();
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

        [HttpPost]
        [Authorize]
        [Route("Category/Order")]
        public IActionResult Order(string ids)
        {
            int[] idsArray = ids.Split(',').Select(int.Parse).ToArray();

            for (int i = 0; i < idsArray.Length; i++)
            {
                var category = _categoryService.GetById(idsArray[i]);

                if (category != null)
                {
                    category.DisplayOrder = i + 1;

                    _categoryService.SaveChanges();

                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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