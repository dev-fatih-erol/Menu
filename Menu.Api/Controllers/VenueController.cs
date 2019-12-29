using System;
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

        // GET venue/5
        [HttpGet]
        [Route("Venue/Filter/{name?}/{venueType?}/{offset?}/{limit?}")]
        public IActionResult Filter(string name, string venueType, int offset, int limit)
        {
            var venues = _venueService.Get();

            if (!string.IsNullOrEmpty(name))
            {
                venues = venues.Where(v => v.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(venueType))
            {
                venues = venues.Where(v => v.VenueType == Core.Enums.VenueType.Cafe);
            }

            venues = venues.Skip(offset).Take(limit);

            if (venues.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<VenueDto>>(venues.ToList())
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Mekan bulunamadı"
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