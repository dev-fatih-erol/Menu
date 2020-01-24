using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class WaiterController : Controller
    {
        private readonly ILogger<WaiterController> _logger;

        private readonly IMapper _mapper;

        private readonly IWaiterService _waiterService;

        public WaiterController(ILogger<WaiterController> logger,
            IMapper mapper,
            IWaiterService waiterService)
        {
            _logger = logger;

            _mapper = mapper;

            _waiterService = waiterService;
        }

        // GET: waiter/5/Tables
        [HttpGet]
        [Route("Waiter/{id:int}/Tables")]
        public IActionResult GetWithTableById(int id)
        {
            var waiters = _waiterService.GetWithTableById(id);

            if (waiters.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = waiters.Select(w => w.TableWaiter.Select(w => new
                    {
                        w.Table.Id,
                        w.Table.Name,
                        w.Table.TableStatus
                    }))
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Masa bulunamadı"
            });
        }
    }
}