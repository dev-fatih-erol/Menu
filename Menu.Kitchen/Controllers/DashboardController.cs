using System.Linq;
using Menu.Core.Enums;
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
                var orders = _orderService.GetByVenueId(kitchen.Venue.Id, OrderStatus.Approved);

                return Ok(orders.Select(order => new
                {
                    order.Id,
                    order.Code,
                    order.Description,
                    order.CreatedDate,
                    orderDetails = order.OrderDetail.Select(orderDetail => new
                    {
                        orderDetail.Id,
                        orderDetail.Name,
                        orderDetail.Photo,
                        orderDetail.OptionItem,
                        orderDetail.Quantity
                    }),
                    User = new
                    {
                        order.OrderTable.User.Name,
                        order.OrderTable.User.Surname,
                        order.OrderTable.User.Photo
                    },
                    Waiter = new
                    {
                        order.OrderWaiter.Waiter.Name,
                        order.OrderWaiter.Waiter.Surname
                    }
                }));
            }

            return NotFound("Mutfak bulunamadı");
        }
    }
}