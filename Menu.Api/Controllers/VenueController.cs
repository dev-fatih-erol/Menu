using System.Net;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class VenueController : Controller
    {
        private readonly ILogger<VenueController> _logger;

        private readonly IVenueService _venueService;

        public VenueController(ILogger<VenueController> logger,
            IVenueService venueService)
        {
            _logger = logger;

            _venueService = venueService;
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
                    Result = venue.ToVenueDto()
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