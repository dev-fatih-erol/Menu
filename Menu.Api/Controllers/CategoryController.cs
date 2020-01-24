using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        private readonly IMapper _mapper;

        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            IMapper mapper,
            ICategoryService categoryService)
        {
            _logger = logger;

            _mapper = mapper;

            _categoryService = categoryService;
        }

        // GET venue/5/menu
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Venue/{venueId:int}/Menu")]
        public IActionResult GetCategoriesAndProductsByVenueId(int venueId)
        {
            var categoriesAndProducts = _categoryService.GetCategoriesAndProductsByVenueId(venueId);

            if (categoriesAndProducts.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<CategoryProductDto>>(categoriesAndProducts)
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
        [Authorize(Roles = "User")]
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
                    Result = _mapper.Map<List<CategoryDto>>(categories)
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
        [Authorize(Roles = "User")]
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
                    Result = _mapper.Map<CategoryDto>(category)
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