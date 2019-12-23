using System.Linq;
using System.Net;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;

        private readonly ICityService _cityService;

        public CityController(ILogger<CityController> logger,
            ICityService cityService)
        {
            _logger = logger;

            _cityService = cityService;
        }

        // GET city/5
        [HttpGet]
        [Route("City/{id:int}")]
        public IActionResult GetById(int id)
        {
            var city = _cityService.GetById(id);

            if (city != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = city.ToCityDto()
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Şehir bulunamadı"
            });
        }

        // GET cities
        [HttpGet]
        [Route("Cities")]
        public IActionResult Get()
        {
            var cities = _cityService.Get();

            if (cities.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = cities.ToCityDto()
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Şehir bulunamadı"
            });
        }
    }
}