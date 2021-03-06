﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Helpers;
using Menu.Api.Models;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
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

        private readonly ITimeLimitedDataProtector _protector;

        private readonly ICityService _cityService;

        private readonly IUserService _userService;

        private readonly IOrderTableService _orderTableService;

        public UserController(ILogger<UserController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IDataProtectionProvider provider,
            ICityService cityService,
            IUserService userService,
            IOrderTableService orderTableService)
        {
            _logger = logger;

            _configuration = configuration;

            _mapper = mapper;

            _protector = provider.CreateProtector(configuration["Keys:VerificationKey"])
                                 .ToTimeLimitedDataProtector();

            _cityService = cityService;

            _userService = userService;

            _orderTableService = orderTableService;
        }

        [HttpGet]
        //[Authorize(Roles = "Waiter")]
        [Route("User/Guest/Table/{tableId:int}")]
        public IActionResult GetGuest(int tableId)
        {
            var orderTable = _orderTableService.GetByGuest(tableId);

            if (orderTable != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new
                    {
                        orderTable.User.Id,
                        orderTable.User.Name
                    }
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
        [Authorize(Roles = "Waiter")]
        [Route("User/Guest")]
        public IActionResult CreateGuest()
        { 
            var random = RandomHelper.Generate(1000, 10000);

            var newUser = new User
            {
                Name = "Misafir-" + random,
                Surname = "M",
                PhoneNumber = "0",
                Password = "0",
                Photo = "https://walldeco.id/themes/walldeco/assets/images/avatar-default.jpg",
                CreatedDate = DateTime.Now,
                CityId = 34,
                Point = 0,
                IsGuest = true
            };

            _userService.Create(newUser);

            _userService.SaveChanges();

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = newUser.Id
            });
        }

        // POST user/changepassword
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("User/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDto dto)
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

            var user = _userService.GetByIdAndPassword(User.Identity.GetId(), dto.OldPassword.ToMD5());

            if (user != null)
            {
                user.Password = dto.NewPassword.ToMD5();

                _userService.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = user != null
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kullanıcı bulunamadı"
            });
        }

        // POST user/createpassword
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("User/CreatePassword")]
        public IActionResult CreatePassword(CreatePasswordDto dto)
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

            var user = _userService.GetById(User.Identity.GetId());

            if (user != null)
            {
                user.Password = dto.Password.ToMD5();

                _userService.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = user != null
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Kullanıcı bulunamadı"
            });
        }

        // GET me
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Me")]
        public IActionResult GetById()
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

        // POST user/auth
        [HttpPost]
        [Route("User/Auth")]
        [AllowAnonymous]
        public IActionResult Auth(AuthUserDto dto)
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
                Message = "Kullanıcı bulunamadı"
            });
        }

        // POST user
        [HttpPost]
        [Route("User")]
        [AllowAnonymous]
        public IActionResult Create(CreateUserDto dto)
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

            if (user == null)
            {
                var city = _cityService.GetById(dto.CityId);

                if (city != null)
                {
                    var newUser = new User
                    {
                        Name = dto.Name.FirstCharToUpper(),
                        Surname = dto.Surname.FirstCharToUpper(),
                        PhoneNumber = dto.PhoneNumber,
                        Password = dto.Password.ToMD5(),
                        Photo = "https://walldeco.id/themes/walldeco/assets/images/avatar-default.jpg",
                        CreatedDate = DateTime.Now,
                        CityId = dto.CityId,
                        Point = 10000,
                        IsGuest = false
                    };

                    _userService.Create(newUser);

                    _userService.SaveChanges();

                    return Ok(new
                    {
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = _mapper.Map<UserDto>(newUser)
                    });
                }

                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Şehir bulunamadı"
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