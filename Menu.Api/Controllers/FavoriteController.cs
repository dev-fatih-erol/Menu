using System;
using System.Net;
using AutoMapper;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ILogger<FavoriteController> _logger;

        private readonly IMapper _mapper;

        private readonly IFavoriteService _favoriteService;

        private readonly IVenueService _venueService;

        public FavoriteController(ILogger<FavoriteController> logger,
            IMapper mapper,
            IFavoriteService favoriteService,
            IVenueService venueService)
        {
            _logger = logger;

            _mapper = mapper;

            _favoriteService = favoriteService;

            _venueService = venueService;
        }

        // POST user/favorite/create
        [HttpPost]
        [Route("User/Favorite/Create")]
        public IActionResult Create([FromForm]int venueId)
        {
            var venue = _venueService.GetById(venueId);

            if (venue != null)
            {
                var favorite = _favoriteService.GetByUserIdAndVenueId(1, venue.Id);

                if (favorite == null)
                {
                    _favoriteService.Create(new Favorite
                    {
                        UserId = 1,
                        VenueId = venue.Id,
                        CreatedDate = DateTime.Now
                    });

                    _favoriteService.SaveChanges();
                }

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = true
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