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

        private readonly IOrderCashService _orderCashService;

        private readonly IOrderPaymentService _orderPaymentService;

        private readonly IVenuePaymentMethodService _venuePaymentMethodService;

        public TableController(ITableService tableService,
            ICashService cashService,
            IWaiterService waiterService,
            IOrderTableService orderTableService,
            IUserService userService,
            IVenueService venueService,
            IVenuePaymentMethodService venuePaymentMethodService,
            IOrderCashService orderCashService,
            IOrderPaymentService orderPaymentService)
        {
            _tableService = tableService;

            _cashService = cashService;

            _waiterService = waiterService;

            _orderTableService = orderTableService;

            _userService = userService;

            _venueService = venueService;

            _venuePaymentMethodService = venuePaymentMethodService;

            _orderCashService = orderCashService;

            _orderPaymentService = orderPaymentService;
        }

        [HttpPost]
        [Authorize]
        [Route("Ajax/Table/ChangeOrderCashStatus")]
        public IActionResult ChangeOrderCashStatus(int id, string orderCashStatus)
        {
            var orderCash = _orderCashService.GetById(id);

            if (orderCash != null)
            {
                orderCash.OrderCashStatus = orderCashStatus.ToOrderCashStatusEnum();

                _orderCashService.SaveChanges();
            }
            
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("Table/Reports")]
        public IActionResult Reports()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Ajax/Table/Reports")]
        public IActionResult AllReports()
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByReports(cash.Venue.Id, true);

            var TotalPriceSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted)
            .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                                 .Select(or => or.OrderDetail
                                                 .Sum(or => or.Price * or.Quantity)).Sum()));

            var vipmisafirpriceSum = string.Format("{0:N2}", orderTables.Where(x =>
             x.OrderCash.OrderCashStatus == OrderCashStatus.Treat)
             .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                                  .Select(or => or.OrderDetail
                                                  .Sum(or => or.Price * or.Quantity)).Sum()));

            var nakitSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Nakit")
            .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                                 .Select(or => or.OrderDetail
                                                 .Sum(or => or.Price * or.Quantity)).Sum()));
            var KrediKartıSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Kredi Kartı")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                          .Select(or => or.OrderDetail
                                          .Sum(or => or.Price * or.Quantity)).Sum()));

            var MultinetSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Multinet")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                         .Select(or => or.OrderDetail
                                         .Sum(or => or.Price * or.Quantity)).Sum()));
            var SodexoSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Sodexo")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                          .Select(or => or.OrderDetail
                                          .Sum(or => or.Price * or.Quantity)).Sum()));
            var TicketRestaurantSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Ticket Restaurant")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                     x.OrderStatus != OrderStatus.Cancel &&
                                     x.OrderStatus != OrderStatus.Pending &&
                                     x.OrderStatus != OrderStatus.Denied)
                                          .Select(or => or.OrderDetail
                                          .Sum(or => or.Price * or.Quantity)).Sum()));
            var SetCardSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "SetCard")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                     x.OrderStatus != OrderStatus.Cancel &&
                                     x.OrderStatus != OrderStatus.Pending &&
                                     x.OrderStatus != OrderStatus.Denied)
                                          .Select(or => or.OrderDetail
                                          .Sum(or => or.Price * or.Quantity)).Sum()));
            var WinwinSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Winwin")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                           .Select(or => or.OrderDetail
                                           .Sum(or => or.Price * or.Quantity)).Sum()));
            var MetropolSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted &&
            x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Metropol")
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                     x.OrderStatus != OrderStatus.Cancel &&
                                     x.OrderStatus != OrderStatus.Pending &&
                                     x.OrderStatus != OrderStatus.Denied)
                                            .Select(or => or.OrderDetail
                                            .Sum(or => or.Price * or.Quantity)).Sum()));
            var KacakSum = string.Format("{0:N2}", orderTables.Where(x =>
            x.OrderCash.OrderCashStatus == OrderCashStatus.NoPayment)
                .Sum(x => x.Order.Where(x => x.Id == x.Id &&
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied)
                                             .Select(or => or.OrderDetail
                                             .Sum(or => or.Price * or.Quantity)).Sum()));

            var KacakSayisi = orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.NoPayment).Count();

            var VipSayisi = orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.Treat).Count();

            var Toplammusteri = orderTables.Count();

            var onlineMusteri = orderTables.Where(x => x.User.IsGuest == false).Count();

            var normalMusteri = orderTables.Where(x => x.User.IsGuest == true).Count();

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = new
                {
                    cashprice = nakitSum,
                    creditcardprice = KrediKartıSum,
                    sodexoprice = SodexoSum,
                    ticketrestaurantprice = TicketRestaurantSum,
                    setcardprice = SetCardSum,
                    winwinprice = WinwinSum,
                    metropolprice = MetropolSum,
                    kacakprice = KacakSum,
                    multinetprice = MultinetSum,
                    totalprice = TotalPriceSum,
                    vipprice = vipmisafirpriceSum,
                    kacaksayisi = KacakSayisi,
                    vipsayisi = VipSayisi,
                    toplammusteri = Toplammusteri,
                    onlinemusteri = onlineMusteri,
                    normalmusteri = normalMusteri,
                }
            });
        }

        [HttpGet]
        [Authorize]
        [Route("Table/Old")]
        public IActionResult Old()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Ajax/Table/Old")]
        public IActionResult OldOrders()
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByOldTableId(cash.Venue.Id, true)
                                           .Where(o => o.Order.Any(o => o.OrderStatus != OrderStatus.Pending)).OrderByDescending(x => x.OrderCash.CreatedDate).ToList();

            return Ok(orderTables.Select(orderTable => new
            {
                orderTable.Id,
                orderTable.IsClosed,
                createdDate = orderTable.CreatedDate.ToString("HH:mm"),
                orderTable.User.Name,
                orderTable.User.Surname,
                orderTable.TableId,
                table = orderTable.Table.Name,
                orderpayment = orderTable.OrderPayment.VenuePaymentMethod.PaymentMethod.Text,
                ordercashId = orderTable.OrderCash.Id,
                ordercash = orderTable.OrderCash.OrderCashStatus.ToOrderCashStatus(),
                cashdate = orderTable.OrderCash.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss"),
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
        [Route("Ajax/User/{id:int}/Orders")]
        public IActionResult UserCheckOut(int id)
        {
            var orderTable = _orderTableService.GetByUserId(id, false);

            if (orderTable != null)
            {
                var venue = _venueService.GetPaymentMethodById(orderTable.VenueId);

                if (venue == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Mekan bulunamadı"
                    });
                }

                var totalPrice = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                             o.OrderStatus != OrderStatus.Denied &&
                                             o.OrderStatus != OrderStatus.Pending)
                                 .Select(o => o.OrderDetail
                                 .Sum(o => o.Price * o.Quantity)).Sum();

                var user = _userService.GetById(id);

                int orderPaymentMethod;
                string realPrice = string.Empty;
                string usedPoint = string.Empty;
                string tip = string.Empty;
                if (orderTable.OrderPayment != null)
                {
                    orderPaymentMethod =
                         _orderPaymentService.GetByOrderTableId(orderTable.Id)
                         .VenuePaymentMethod.PaymentMethod.Id;

                    var tlPoint =  Convert.ToInt32(orderTable.OrderPayment.UsedPoint * 0.001);

                    realPrice = string.Format("{0:N2}", (totalPrice + orderTable.OrderPayment.Tip) - tlPoint);

                    usedPoint = string.Format("{0:N2}", Convert.ToInt32(orderTable.OrderPayment.UsedPoint * 0.001));

                    tip = string.Format("{0:N2}", orderTable.OrderPayment.Tip);
                }
                else
                {
                    orderPaymentMethod = venue.VenuePaymentMethod.FirstOrDefault().PaymentMethod.Id;

                    realPrice = string.Format("{0:N2}", totalPrice);

                    usedPoint = string.Format("{0:N2}", 0);

                    tip = string.Format("{0:N2}", 0);
                }
                
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new
                    {
                        PaymentMethods = venue.VenuePaymentMethod.Select(v => new
                        {
                            v.PaymentMethod.Id,
                            v.PaymentMethod.Text
                        }),
                        OrderPaymentMethod = orderPaymentMethod,
                        UsedPoint = usedPoint,
                        Tip = tip,
                        TLPoint = string.Format("{0:N2}", user.Point * 0.001),
                        TotalPrice = string.Format("{0:N2}", totalPrice),
                        RealPrice = realPrice
                    }
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Sipariş bulunamadı"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("Ajax/Table/{tableId:int}/orders")]
        public IActionResult TableOrders(int tableId)
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByTableId(cash.Venue.Id,tableId, false)
                                           .Where(o => o.Order.Any(o => o.OrderStatus != OrderStatus.Pending)).ToList();

            return Ok(orderTables.Select(orderTable => new
            {
                orderTable.Id,
                orderTable.IsClosed,
                createdDate = orderTable.CreatedDate.ToString("HH:mm"),
                userId = orderTable.User.Id,
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