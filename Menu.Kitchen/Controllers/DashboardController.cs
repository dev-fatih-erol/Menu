using System.Linq;
using Menu.Kitchen.Extensions;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Kitchen.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;

        private readonly IKitchenService _kitchenService;

        public DashboardController(IOrderService orderService,
            IKitchenService kitchenService)
        {
            _orderService = orderService;

            _kitchenService = kitchenService;
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
        [Route("Orders")]
        public IActionResult Orders()
        {
            var kitchen = _kitchenService.GetById(User.Identity.GetId());

            if (kitchen != null)
            {
                var orders = _orderService.GetByVenueId(kitchen.Venue.Id);

                return Ok(orders.Select(order => order.Id));
            }

            return NotFound("Mutfak bulunamadı");
        }
    }
}