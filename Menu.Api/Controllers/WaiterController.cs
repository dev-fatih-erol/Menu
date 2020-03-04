﻿using System;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    public class WaiterController : Controller
    {
        private readonly ILogger<WaiterController> _logger;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        private readonly IWaiterService _waiterService;

        private readonly ITableWaiterService _tableWaiterService;

        private readonly string _key = "key=AAAA7Tr-w-A:APA91bFkdAPrjKgsrKdzqFpR1EXzmie3oUk6KaVgaPmdCyNdOsik_zyMJZHo2MgAAXYShzwJjj1dnlPpn-DvhW5JnYyzwDyahdVV9FyoHYV4K6XUggKJTm0uXRLxVhodorwKEzThBkqc";

        public WaiterController(ILogger<WaiterController> logger,
            IMapper mapper,
            IConfiguration configuration,
            IWaiterService waiterService,
            ITableWaiterService tableWaiterService)
        {
            _logger = logger;

            _mapper = mapper;

            _configuration = configuration;

            _waiterService = waiterService;

            _tableWaiterService = tableWaiterService;
        }

        [HttpGet]
        [Route("Waiter/Call")]
        [Throttle(Name = "Throttle", Seconds = 300)]
        public async Task<IActionResult> CallWaiter(int tableId)
        {
            var waiters = _tableWaiterService.GetByTableId(tableId);

            var tokens = waiters.Select(s => s.Waiter.WaiterToken.Token).ToList();

            if (tokens.Count() > 0 || tokens != null)
            {
                dynamic foo = new ExpandoObject();
                foo.registration_ids = tokens;
                foo.notification = new
                {
                    title = "Çağrı var",
                    body = waiters.Select(x => x.Table.Name).FirstOrDefault() + " isimli masadan çağrı var",
                    data = new { tableId }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                await response.Content.ReadAsStringAsync();

                return Ok(true);
            }

            return Ok(false);
        }

        [HttpGet]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Get/Me")]
        public IActionResult GetById()
        {
            var waiter = _waiterService.GetById(User.Identity.GetId());

            if (waiter != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new
                    {
                        waiter.Id,
                        waiter.Name,
                        waiter.Surname,
                        waiter.VenueId
                    }
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Garson bulunamadı"
            });
        }

        // GET: waiter/tables
        [HttpGet]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Tables")]
        public IActionResult GetByWaiterId()
        {
            var tables = _tableWaiterService.GetByWaiterId(User.Identity.GetId());

            if (tables.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = tables.Select(t => new {
                        t.Table.Id,
                        t.Table.Name,
                        t.Table.TableStatus
                    })
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Masa bulunamadı"
            });
        }

        // POST waiter/auth
        [HttpPost]
        [Route("Waiter/Auth")]
        [AllowAnonymous]
        public IActionResult Auth(AuthWaiterDto dto)
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

            var waiter = _waiterService.GetByUsernameAndPassword(dto.Username, dto.Password.ToMD5());

            if (waiter != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, waiter.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Waiter")
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
                Message = "Garson bulunamadı"
            });
        }
    }
}