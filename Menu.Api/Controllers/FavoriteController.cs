using System;
using System.Net;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
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

        // GET user/favorite/venue/5/status
        [HttpGet]
        [Authorize]
        [Route("User/Favorite/Venue/{venueId:int}/Status")]
        public IActionResult Status(int venueId)
        {
            var venue = _venueService.GetById(venueId);

            if (venue != null)
            {
                var favorite = _favoriteService.GetByUserIdAndVenueId(User.Identity.GetId(), venue.Id);

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = favorite != null
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Mekan bulunamadı"
            });
        }

        // POST user/favorite/delete
        [HttpPost]
        [Authorize]
        [Route("User/Favorite/Delete")]
        public IActionResult Delete([FromForm]int venueId)
        {
            var venue = _venueService.GetById(venueId);

            if (venue != null)
            {
                var favorite = _favoriteService.GetByUserIdAndVenueId(User.Identity.GetId(), venue.Id);

                if (favorite != null)
                {
                    _favoriteService.Delete(favorite);

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

        // POST user/favorite/create
        [HttpPost]
        [Authorize]
        [Route("User/Favorite/Create")]
        public IActionResult Create([FromForm]int venueId)
        {
            var venue = _venueService.GetById(venueId);

            if (venue != null)
            {
                var favorite = _favoriteService.GetByUserIdAndVenueId(User.Identity.GetId(), venue.Id);

                if (favorite == null)
                {
                    _favoriteService.Create(new Favorite
                    {
                        UserId = User.Identity.GetId(),
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