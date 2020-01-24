using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Helpers;
using Menu.Api.Models;
using Menu.Api.Services;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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

        // POST verification/code/check
        [HttpPost]
        [AllowAnonymous]
        [Route("Verification/Code/Check")]
        public IActionResult CheckCode(CheckCodeDto dto)
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

            string decryptedToken;

            try
            {
                decryptedToken = _protector.Unprotect(dto.Token);
            }
            catch
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Güvenlik anahtarı doğrulanamadı"
                });
            }

            if (!decryptedToken.VerifyPhoneNumber(dto.PhoneNumber, dto.Code))
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Kimlik doğrulanamadı"
                });
            }

            var user = _userService.GetByPhoneNumber(dto.PhoneNumber);

            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "User")
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = tokenHandler.WriteToken(token)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kullanıcı Bulunamadı"
            });
        }

        // POST verification/phonenumber/check
        [HttpPost]
        [AllowAnonymous]
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

        // POST verification/code/verify
        [HttpPost]
        [AllowAnonymous]
        [Route("Verification/Code/Verify")]
        public IActionResult VerifyCode(VerifyCodeDto dto)
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

            string decryptedToken;

            try
            {
                decryptedToken = _protector.Unprotect(dto.Token);
            }
            catch
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Güvenlik anahtarı doğrulanamadı"
                });
            }

            if (!decryptedToken.VerifyPhoneNumber(dto.PhoneNumber, dto.Code))
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Kimlik doğrulanamadı"
                });
            }

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = _protector.Protect($"{dto.PhoneNumber},{dto.Code}", TimeSpan.FromDays(1))
            });
        }

        // POST verification/code/send
        [HttpPost]
        [AllowAnonymous]
        [Route("Verification/Code/Send")]
        public async Task<IActionResult> SendCode(SendCodeDto dto)
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