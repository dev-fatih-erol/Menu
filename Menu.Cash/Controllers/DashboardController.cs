using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu.Cash.Extensions;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Cash.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITableService _tableService;

        private readonly ICashService _cashService;

        public DashboardController(ITableService tableService,
            ICashService cashService)
        {
            _tableService = tableService;

            _cashService = cashService;
        }

        [HttpGet]
        [Authorize]
        [Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Tables")]
        public IActionResult Tables()
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            if (cash != null)
            {
                var tables = _tableService.GetByVenueId(cash.Venue.Id);

                return Ok(tables.Select(table => new
                {
                    table.Id,
                    table.Name,
                    tableStatus = table.TableStatus.ToTableStatus()
                }));
            }

            return NotFound("Kasa bulunamadı");
        }
    }
}