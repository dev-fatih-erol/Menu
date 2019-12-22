using System.Linq;
using System.Net;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;

            _categoryService = categoryService;
        }

        // GET venue/5/categories/products
        [HttpGet]
        [Route("Venue/{venueId:int}/Categories/Products")]
        public IActionResult GetCategoriesAndProductsByVenueId(int venueId)
        {
            var categoriesAndProducts = _categoryService.GetCategoriesAndProductsByVenueId(venueId);

            if (categoriesAndProducts.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = categoriesAndProducts
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kategori bulunamadı"
            });
        }

        // GET venue/5/categories
        [HttpGet]
        [Route("Venue/{venueId:int}/Categories")]
        public IActionResult GetByVenueId(int venueId)
        {
            var categories = _categoryService.GetByVenueId(venueId);

            if (categories.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = categories.ConvertToDto()
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kategori bulunamadı"
            });
        }

        // GET category/5
        [HttpGet]
        [Route("Category/{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetById(id);

            if (category != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = category.ConvertToDto()
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kategori bulunamadı"
            });
        }
    }
}