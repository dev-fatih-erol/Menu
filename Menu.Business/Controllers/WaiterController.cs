using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.WaiterViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class WaiterController : Controller
    {
        private readonly IWaiterService _waiterService;

        private readonly ITableWaiterService _tableWaiterService;

        public WaiterController(IWaiterService waiterService,
           ITableWaiterService tableWaiterService)
        {
            _waiterService = waiterService;

            _tableWaiterService = tableWaiterService;
        }

        [HttpGet]
        [Authorize]
        [Route("Waiter")]
        public IActionResult Index()
        {
            var model = _waiterService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Username = x.Username,
                CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                Tables = _tableWaiterService.GetByWaiterId(x.Id).Select(x => x.Table.Name)
            });

            return View(model);
        }
    }
}