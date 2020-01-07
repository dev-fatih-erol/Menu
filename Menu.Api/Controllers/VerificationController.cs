using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Helpers;
using Menu.Api.Models;
using Menu.Api.Services;
using Menu.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class VerificationController : Controller
    {
        private readonly ILogger<VerificationController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        private readonly ITimeLimitedDataProtector _protector;

        private readonly ISmsSender _smsSender;

        private readonly IUserService _userService;

        public VerificationController(ILogger<VerificationController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IDataProtectionProvider provider,
            ISmsSender smsSender,
            IUserService userService)
        {
            _logger = logger;

            _configuration = configuration;

            _mapper = mapper;

            _protector = provider.CreateProtector(configuration["Keys:VerificationKey"])
                                 .ToTimeLimitedDataProtector();

            _smsSender = smsSender;

            _userService = userService;
        }

        // POST verification/phonenumber/check
        [HttpPost]
        [Route("Verification/PhoneNumber/Check")]
        public async Task<IActionResult> CheckPhoneNumber(CheckPhoneNumberDto dto)
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

            var user = _userService.GetByPhoneNumber(dto.PhoneNumber);

            if (user != null)
            {
                var code = RandomHelper.Generate(1000, 9999);

                await _smsSender.Send(dto.PhoneNumber, $"Bimenü doğrulama kodunuz: {code}");

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _protector.Protect($"{dto.PhoneNumber},{code}", TimeSpan.FromMinutes(3))
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kullanıcı Bulunamadı"
            });
        }

        // POST verification/code/send
        [HttpPost]
        [Route("Verification/Code/Send")]
        public async Task<IActionResult> Send(SendVerificationCodeDto dto)
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

            var user = _userService.GetByPhoneNumber(dto.PhoneNumber);

            if (user == null)
            {
                var code = RandomHelper.Generate(1000, 9999);

                await _smsSender.Send(dto.PhoneNumber, $"Bimenü doğrulama kodunuz: {code}");

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _protector.Protect($"{dto.PhoneNumber},{code}", TimeSpan.FromMinutes(3))
                });
            }

            return BadRequest(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Kullanıcı mevcut"
            });
        }
    }
}