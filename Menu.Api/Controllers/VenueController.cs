using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Core.Enums;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class VenueController : Controller
    {
        private readonly ILogger<VenueController> _logger;

        private readonly IMapper _mapper;

        private readonly IVenueService _venueService;

        private readonly IVenueFeatureService _venueFeaturervice;

        private readonly IVenuePaymentMethodService _venuePaymentMethodrvice;

        public VenueController(ILogger<VenueController> logger,
            IMapper mapper,
            IVenueService venueService,
            IVenueFeatureService venueFeatureService,
            IVenuePaymentMethodService venuePaymentMethodService)
        {
            _logger = logger;

            _mapper = mapper;

            _venueService = venueService;

            _venueFeaturervice = venueFeatureService;

            _venuePaymentMethodrvice = venuePaymentMethodService;
        }

        // GET venue/5/features
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Venue/{venueId:int}/features")]
        public IActionResult GetByVenueId(int venueId)
        {
            var features = _venueFeaturervice.GetByVenueId(venueId)
                .Select(f => new
                {
                    f.Feature.Id,
                    f.Feature.Text
                }).ToList();

            var paymentMethods = _venuePaymentMethodrvice.GetByVenueId(venueId).Select(v => new
            {
                v.PaymentMethod.Id,
                v.PaymentMethod.Text
            }).ToList();
            
            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = new
                {
                    features,
                    paymentMethods
                }
            });
        }

        // GET venue/filter
        [HttpGet]
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
        [Route("Venue/Random")]
        public IActionResult GetRandom(VenueType? venueType, int limit = 5)
        {
            var venues = _venueService.GetRandom(venueType, limit);

            if (venues.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<RandomVenueDto>>(venues)
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
        [Authorize(Roles = "User")]
        [Route("Venue/{id:int}/Details")]
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
        [Authorize(Roles = "User")]
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