using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Core.Enums;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class VenueController : Controller
    {
        private readonly ILogger<VenueController> _logger;

        private readonly IMapper _mapper;

        private readonly IVenueService _venueService;

        public VenueController(ILogger<VenueController> logger,
            IMapper mapper,
            IVenueService venueService)
        {
            _logger = logger;

            _mapper = mapper;

            _venueService = venueService;
        }

        // GET venue/filter
        [HttpGet]
        [Route("Venue/Filter")]
        public IActionResult GetByCriteria(string name)
        {
            var venues = _venueService.GetByCriteria(name);

            if (venues.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<VenueDto>>(venues)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Mekan bulunamadı"
            });
        }

        // GET venue/random
        [HttpGet]
        [Route("Venue/Random")]
        public IActionResult GetRandom(VenueType? venueType, int limit = 5)
        {
            var venues = _venueService.GetRandom(venueType, limit);

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = _mapper.Map<List<RandomVenueDto>>(venues)
            });
        }

        // GET venue/5/details
        [HttpGet]
        [Route("Venue/{id:int}/details")]
        public IActionResult GetDetailById(int id)
        {
            var venue = _venueService.GetDetailById(id);

            if (venue != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<VenueDetailDto>(venue)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Mekan bulunamadı"
            });
        }

        // GET venue/5
        [HttpGet]
        [Route("Venue/{id:int}")]
        public IActionResult GetById(int id)
        {
            var venue = _venueService.GetById(id);

            if (venue != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<VenueDto>(venue)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Mekan bulunamadı"
            });
        }
    }
}