using System;
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
    public class SuggestionComplaintController : Controller
    {
        private readonly ILogger<SuggestionComplaintController> _logger;

        private readonly IMapper _mapper;

        private readonly ISuggestionComplaintService _suggestionComplaintService;

        public SuggestionComplaintController(ILogger<SuggestionComplaintController> logger,
            IMapper mapper,
            ISuggestionComplaintService suggestionComplaintService)
        {
            _logger = logger;

            _mapper = mapper;

            _suggestionComplaintService = suggestionComplaintService;
        }

        // POST suggestioncomplaint
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("SuggestionComplaint")]
        public IActionResult Create(CreateSuggestionComplaintDto dto)
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

            var newSuggestionComplaint = new SuggestionComplaint
            {
                SubjectType = (SubjectType)dto.SubjectType,
                Description = dto.Description,
                SuggestionComplaintStatus = SuggestionComplaintStatus.Pending,
                CreatedDate = DateTime.Now,
                UserId = User.Identity.GetId()
            };

            _suggestionComplaintService.Create(newSuggestionComplaint);

            _suggestionComplaintService.SaveChanges();

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = true
            });
        }
    }
}