using System.Collections.Generic;
using System.Linq;
using Menu.Core.Enums;
using Menu.Core.Models;
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


        [HttpPost]
        [Authorize]
        [Route("Order/{id:int}")]
        public IActionResult UpdateOrderStatus(int id, OrderStatus orderStatus)
        {
            var order = _orderService.GetById(id);

            if (order != null)
            {
                order.OrderStatus = orderStatus;

                _orderService.SaveChanges();

                return Ok(true);
            }

            return NotFound("Sipariş bulunamadı");
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
        public IActionResult Orders(OrderStatus orderStatus = OrderStatus.Approved)
        {
            var kitchen = _kitchenService.GetById(User.Identity.GetId());

            if (kitchen != null)
            {
                if (orderStatus == OrderStatus.Approved ||
                    orderStatus == OrderStatus.Preparing ||
                    orderStatus == OrderStatus.Prepared)
                {

                    var orders = _orderService.GetByVenueId(kitchen.Venue.Id, orderStatus);

                    return Ok(orders.Select(order => new
                    {
                        order.Id,
                        order.Code,
                        order.Description,
                        OrderStatus = order.OrderStatus.GetDescription(),
                        CreatedDate = order.CreatedDate.ToString("HH:mm"),
                        orderDetails = order.OrderDetail.Select(orderDetail => new
                        {
                            orderDetail.Id,
                            orderDetail.Name,
                            orderDetail.Photo,
                            orderDetail.OptionItem,
                            orderDetail.Quantity
                        }),
                        Table = new
                        {
                            order.OrderTable.Table.Name,
                        },
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

                return NotFound("Sipariş tipi bulunamadı");
            }

            return NotFound("Mutfak bulunamadı");
        }
    }
}