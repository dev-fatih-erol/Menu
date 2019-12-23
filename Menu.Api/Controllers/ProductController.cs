using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private readonly IMapper _mapper;

        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger,
            IMapper mapper,
            IProductService productService)
        {
            _logger = logger;

            _mapper = mapper;

            _productService = productService;
        }

        // GET product/5/details
        [HttpGet]
        [Route("Product/{id:int}/Details")]
        public IActionResult GetDetailById(int id)
        {
            var product = _productService.GetDetailById(id);

            if (product != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<ProductDetailDto>(product)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }

        // GET category/5/products
        [HttpGet]
        [Route("Category/{categoryId:int}/Products")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            var products = _productService.GetByCategoryId(categoryId);

            if (products.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<ProductDto>>(products)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }

        // GET product/5
        [HttpGet]
        [Route("Product/{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);

            if (product != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<ProductDto>(product)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }
    }
}