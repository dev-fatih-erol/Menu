using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Models;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class CommentRatingController : Controller
    {
        private readonly ILogger<CommentRatingController> _logger;

        private readonly IMapper _mapper;

        private readonly ICommentRatingService _commentRatingService;

        private readonly IOrderTableService _orderTableService;

        private readonly IOrderCashService _orderCashService;

        public CommentRatingController(ILogger<CommentRatingController> logger,
            IMapper mapper,
            ICommentRatingService commentRatingService,
            IOrderTableService orderTableService,
            IOrderCashService orderCashService)
        {
            _logger = logger;

            _mapper = mapper;

            _commentRatingService = commentRatingService;

            _orderTableService = orderTableService;

            _orderCashService = orderCashService;
        }

        // GET Comment/5
        [HttpGet]
        [Authorize]
        [Route("Comment/{orderTableId:int}")]
        public IActionResult GetByUserIdAndOrderTableId(int orderTableId)
        {
            var commentRating = _commentRatingService.GetByUserIdAndOrderTableId(User.Identity.GetId(), orderTableId);

            if (commentRating != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new
                    {
                        commentRating.Id,
                        commentRating.Text,
                        commentRating.Speed,
                        commentRating.Waiter,
                        commentRating.Flavor,
                        commentRating.CreatedDate
                    }
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Yorum bulunamadı"
            });
        }

        // GET Venue/5/Comments
        [HttpGet]
        [Authorize]
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

        // POST Comment
        [HttpPost]
        [Authorize]
        [Route("Comment")]
        public IActionResult Create(CreateCommentRatingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = ModelState.GetErrors()
                });
            }

            var orderTable = _orderTableService.GetById(dto.OrderTableId, User.Identity.GetId(), true);

            if (orderTable == null)
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Sipariş bulunamadı"
                });
            }

            var commentRating = _commentRatingService.GetByUserIdAndOrderTableId(User.Identity.GetId(), dto.OrderTableId);

            if (commentRating != null)
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bu siparişe daha önce yorum yaptınız"
                });
            }

            var orderCash = _orderCashService.GetByUserIdAndOrderTableId(User.Identity.GetId(), dto.OrderTableId);

            if (orderCash == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Ödeme işlemini gerçekleştirdikten sonra yorum veya puanlama işlemi yapabilirsiniz"
                });
            }

            if (orderCash.OrderCashStatus == OrderCashStatus.NoPayment)
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Ödeme işlemini gerçekleştirmediğiniz için yorum yapamazsınız"
                });
            }

            if (DateTime.Now > orderTable.CreatedDate.AddDays(15))
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Sipariş tarihinden 15 gün içinde yorum ve puanlama yapabilirsiniz"
                });
            }

            var newCommentRating = new CommentRating
            {
                Text = dto.Text ?? null,
                Speed = dto.Speed,
                Waiter = dto.Waiter,
                Flavor = dto.Flavor,
                CreatedDate = DateTime.Now,
                VenueId = orderTable.VenueId,
                OrderCashId = orderCash.Id,
                UserId = User.Identity.GetId(),          
            };

            _commentRatingService.Create(newCommentRating);

            _commentRatingService.SaveChanges();

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = true
            });
        }
    }
}