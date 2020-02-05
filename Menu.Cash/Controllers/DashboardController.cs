using System.Linq;
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

        private readonly IWaiterService _waiterService;

        public DashboardController(ITableService tableService,
            ICashService cashService,
            IWaiterService waiterService)
        {
            _tableService = tableService;

            _cashService = cashService;

            _waiterService = waiterService;
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

        [HttpGet]
        [Authorize]
        [Route("Waiters")]
        public IActionResult Waiters()
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            if (cash != null)
            {
                var waiters = _waiterService.GetByVenueId(cash.Venue.Id);
      
                return Ok(waiters.Select(waiter => new
                {
                    waiter.Id,
                    waiter.Name,
                    waiter.Surname
                }));
            }

            return NotFound("Kasa bulunamadı");
        }
    }
}