using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Menu.Cash.Extensions;
using Menu.Core.Enums;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Cash.Controllers
{
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        private readonly ICashService _cashService;

        private readonly IWaiterService _waiterService;

        private readonly IOrderTableService _orderTableService;

        private readonly IUserService _userService;

        private readonly IVenueService _venueService;

        private readonly IVenuePaymentMethodService _venuePaymentMethodService;

        public TableController(ITableService tableService,
            ICashService cashService,
            IWaiterService waiterService,
            IOrderTableService orderTableService,
            IUserService userService,
            IVenueService venueService,
            IVenuePaymentMethodService venuePaymentMethodService)
        {
            _tableService = tableService;

            _cashService = cashService;

            _waiterService = waiterService;

            _orderTableService = orderTableService;

            _userService = userService;

            _venueService = venueService;

            _venuePaymentMethodService = venuePaymentMethodService;
        }

        [HttpGet]
        [Authorize]
        [Route("Table/{tableId:int}/Detail")]
        public IActionResult Detail()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Table/{tableId:int}/orders")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Ajax/Table/{tableId:int}/orders")]
        public IActionResult TableOrders(int tableId)
        {
            var orderTables = _orderTableService.GetByTableId(tableId, false)
                                           .Where(o => o.Order.Any(o => o.OrderStatus != OrderStatus.Pending)).ToList();

            return Ok(orderTables.Select(orderTable => new
            {
                orderTable.Id,
                orderTable.IsClosed,
                createdDate = orderTable.CreatedDate.ToString("HH:mm"),
                orderTable.User.Name,
                orderTable.User.Surname,
                Orders = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Pending).Select(o => new
                {
                    o.Id,
                    o.Code,
                    o.Description,
                    orderStatus = o.OrderStatus.ToOrderStatus(),
                    o.OrderWaiter.Waiter.Name,
                    o.OrderWaiter.Waiter.Surname,
                    OrderDetails = o.OrderDetail.Select(o => new
                    {
                        o.Id,
                        o.Name,
                        o.Photo,
                        o.Quantity,
                        price = string.Format("{0:N2}", o.Price),
                        o.OptionItem,
                    }).ToList(),
                    TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(or => or.Id == o.Id &&
                                                              or.OrderStatus != OrderStatus.Cancel &&
                                                              o.OrderStatus != OrderStatus.Pending &&
                                                              or.OrderStatus != OrderStatus.Denied)
                                                 .Select(or => or.OrderDetail
                                                 .Sum(or => or.Price * or.Quantity)).Sum())
                }).ToList(),
                TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                         o.OrderStatus != OrderStatus.Pending &&
                                                         o.OrderStatus != OrderStatus.Denied)
                                                     .Select(o => o.OrderDetail
                                                     .Sum(o => o.Price * o.Quantity)).Sum())
            }));
        }
    }
}