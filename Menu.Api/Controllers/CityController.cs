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
    public class CityController : Controller
    {
        private readonly ILogger<CityController> _logger;

        private readonly IMapper _mapper;

        private readonly ICityService _cityService;

        public CityController(ILogger<CityController> logger,
            IMapper mapper,
            ICityService cityService)
        {
            _logger = logger;

            _mapper = mapper;

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
                    Result = _mapper.Map<CityDto>(city)
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
                    Result = _mapper.Map<List<CityDto>>(cities)
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