using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Menu.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IUserService userService)
        {
            _logger = logger;

            _configuration = configuration;

            _mapper = mapper;

            _userService = userService;
        }

        // GET user/me
        [HttpGet]
        [Authorize]
        [Route("User/Me")]
        public IActionResult GetMe()
        {
            var user = _userService.GetById(User.Identity.GetId());

            if (user != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = _mapper.Map<UserDto>(user)
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kullanıcı bulunamadı"
            });
        }

        [HttpPost]
        [Route("Auth")]
        [AllowAnonymous]
        public IActionResult Authenticate(AuthenticateDto dto)
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

            var user = _userService.GetByPhoneNumberAndPassword(dto.PhoneNumber, dto.Password.ToMD5());

            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
                Message = "Kullanıcı bulunamadı"
            });
        }
    }
}