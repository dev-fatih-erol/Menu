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
    public class CommentRatingController : Controller
    {
        private readonly ILogger<CommentRatingController> _logger;

        private readonly IMapper _mapper;

        private readonly ICommentRatingService _commentRatingService;

        public CommentRatingController(ILogger<CommentRatingController> logger,
            IMapper mapper,
            ICommentRatingService commentRatingService)
        {
            _logger = logger;

            _mapper = mapper;

            _commentRatingService = commentRatingService;
        }

        [HttpGet]
        [Route("Venue/{venueId:int}/Comments")]
        public IActionResult GetByVenueId(int venueId)
        {
            var commentRatings = _commentRatingService.GetByVenueId(venueId);

            if (commentRatings.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<List<CommentRatingDto>>(commentRatings)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Yorum bulunamadı"
            });
        }
    }
}