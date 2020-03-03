using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Menu.Cash.Extensions;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Menu.Cash.Controllers
{
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        private readonly ICashService _cashService;

        private readonly IWaiterService _waiterService;

        private readonly IOrderTableService _orderTableService;

        private readonly IUserService _userService;

        private readonly IUserTokenService _userTokenService;

        private readonly IVenueService _venueService;

        private readonly IOrderCashService _orderCashService;

        private readonly IOrderPaymentService _orderPaymentService;

        private readonly IVenuePaymentMethodService _venuePaymentMethodService;

        private readonly IDayReportsService _dayReportsService;

        private readonly string _key = "key=AAAA7Tr-w-A:APA91bFkdAPrjKgsrKdzqFpR1EXzmie3oUk6KaVgaPmdCyNdOsik_zyMJZHo2MgAAXYShzwJjj1dnlPpn-DvhW5JnYyzwDyahdVV9FyoHYV4K6XUggKJTm0uXRLxVhodorwKEzThBkqc";

        public TableController(ITableService tableService,
            ICashService cashService,
            IWaiterService waiterService,
            IOrderTableService orderTableService,
            IUserService userService,
            IVenueService venueService,
            IVenuePaymentMethodService venuePaymentMethodService,
            IOrderCashService orderCashService,
            IOrderPaymentService orderPaymentService,
            IUserTokenService userTokenService,
            IDayReportsService dayReportsService)
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

            _userTokenService = userTokenService;

            _dayReportsService = dayReportsService;
        }

        [HttpPost]
        [Authorize]
        [Route("Table/DayReports")]
        public IActionResult DayReport()
        {

            var cash = _cashService.GetById(User.Identity.GetId());

            _dayReportsService.Create(new DayReports
            { VenueId = cash.VenueId, CreatedDate = DateTime.Now });
            _dayReportsService.SaveChanges();

            return Ok(true);
        }

        [HttpPost]
        [Authorize]
        [Route("Ajax/User/{id:int}/Orders")]
        public async Task<IActionResult> UserCheckOut(int id, int paymentMethod, int cashStatus, int cashId)
        {
            var orderTable = _orderTableService.GetByUserId(id, false);

            if (orderTable != null)
            {
                var totalPrice = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
         o.OrderStatus != OrderStatus.Denied &&
         o.OrderStatus != OrderStatus.Pending)
        .Select(o => o.OrderDetail
        .Sum(o => o.Price * o.Quantity)).Sum();
                if (orderTable.OrderPayment == null)
                {
                    var newOrderPayment = new OrderPayment
                    {
                        VenuePaymentMethodId = paymentMethod,
                        Tip = 0,
                        EarnedPoint = OrderCashStatus.NoPayment == (OrderCashStatus)cashStatus ? 0 : Convert.ToInt32(totalPrice) * 10,
                        UsedPoint = 0,
                        CreatedDate = DateTime.Now,
                        OrderTableId = orderTable.Id
                    };

                    _orderPaymentService.Create(newOrderPayment);

                    _orderPaymentService.SaveChanges();

                    var newOrderCash1 = new OrderCash
                    {
                        CashId = cashId,
                        OrderTableId = orderTable.Id,
                        OrderCashStatus = (OrderCashStatus)cashStatus,
                        CreatedDate = DateTime.Now
                    };

                    _orderCashService.Create(newOrderCash1);

                    _cashService.SaveChanges();

                    var orderTablenNew1 = _orderTableService.GetById(orderTable.Id, id, false);

                    orderTablenNew1.IsClosed = true;

                    _orderTableService.SaveChanges();

                    var user1 = _userService.GetById(id);

                    var point1 = OrderCashStatus.NoPayment == (OrderCashStatus)cashStatus ? 0 : Convert.ToInt32(totalPrice) * 10;

                    user1.Point += point1;

                    _userService.SaveChanges();

                    var orderTableIsLast1 = _orderTableService.GetByTableId(orderTable.TableId, false);

                    if (orderTableIsLast1.Count() == 0)
                    {
                        var table = _tableService.GetById(orderTable.TableId);

                        table.TableStatus = TableStatus.Closed;

                        _tableService.SaveChanges();
                    }

                    var userToken1 = _userTokenService.GetByUserId(id);

                    if (userToken1 != null)
                    {
                        var orderCash = (OrderCashStatus)cashStatus;
                        string[] test = new string[1];
                        test[0] = userToken1.Token;
                        dynamic foo = new ExpandoObject();
                        foo.registration_ids = test;
                        foo.notification = new
                        {
                            title = "Hesap Bilgisi",
                            body = "Hesabınız " + orderCash.ToOrderCashStatus() + ". Bu mekanı değerlendirebilirsiniz.",
                            data = new { orderTable.Id }
                        };

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                        using var httpClient = new HttpClient();

                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                        var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                        await response.Content.ReadAsStringAsync();
                    }
                    return Ok(new
                    {
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = true
                    });
                }

                var orderPayment = _orderPaymentService.GetById(orderTable.OrderPayment.Id);

                orderPayment.VenuePaymentMethodId = paymentMethod;
                orderPayment.EarnedPoint = OrderCashStatus.NoPayment == (OrderCashStatus)cashStatus ? 0 : Convert.ToInt32(totalPrice) * 10;
                _orderPaymentService.SaveChanges();

                var newOrderCash = new OrderCash
                {
                    CashId = cashId,
                    OrderTableId = orderTable.Id,
                    OrderCashStatus = (OrderCashStatus)cashStatus,
                    CreatedDate = DateTime.Now
                };

                _orderCashService.Create(newOrderCash);

                _cashService.SaveChanges();

                var orderTablenNew = _orderTableService.GetById(orderTable.Id, id, false);

                orderTablenNew.IsClosed = true;

                _orderTableService.SaveChanges();

                var user = _userService.GetById(id);

                var point = OrderCashStatus.NoPayment == (OrderCashStatus)cashStatus ? 0 : Convert.ToInt32(totalPrice) * 10;

                user.Point += point;

                _userService.SaveChanges();

                var orderTableIsLast = _orderTableService.GetByTableId(orderTable.TableId, false);

                if (orderTableIsLast.Count() == 0)
                {
                    var table = _tableService.GetById(orderTable.TableId);

                    table.TableStatus = TableStatus.Closed;

                    _tableService.SaveChanges();
                }

                var userToken = _userTokenService.GetByUserId(id);

                if (userToken != null)
                {
                    var orderCash = (OrderCashStatus)cashStatus;
                    string[] test = new string[1];
                    test[0] = userToken.Token;
                    dynamic foo = new ExpandoObject();
                    foo.registration_ids = test;
                    foo.notification = new
                    {
                        title = "Hesap Bilgisi",
                        body = "Hesabınız " + orderCash.ToOrderCashStatus() + ". Bu mekanı değerlendirebilirsiniz.",
                        data = new { orderTable.Id }
                    };

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    using var httpClient = new HttpClient();

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                    var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                    await response.Content.ReadAsStringAsync();
                }

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = true
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Sipariş bulunamadı"
            });
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
            var cash = _cashService.GetById(User.Identity.GetId());

            var dayreport = _dayReportsService.GetByAllReport(cash.Venue.Id);

            ViewBag.Dates = dayreport.Select(x => new SelectListItem
            {
                Text = x.CreatedDate.ToString(),
                Value = x.CreatedDate.ToString()
            });


            ViewBag.Now = DateTime.Now;

            return View();
        }


        [HttpGet]
        [Authorize]
        [Route("Ajax/Table/Reports")]
        public IActionResult AllReports(string date, string endDate)
        {
            var dt = DateTime.Parse(date);

            var endDt = DateTime.Parse(endDate);

            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByReports(cash.Venue.Id, true);

            var TotalPriceSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt)
                .Select(x => x.Order.Where(x =>
                                    x.OrderStatus != OrderStatus.Cancel &&
                                    x.OrderStatus != OrderStatus.Pending &&
                                    x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());

            var vipmisafirpriceSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.Treat && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt)
                .Select(x => x.Order.Where(x =>
                                     x.OrderStatus != OrderStatus.Cancel &&
                                     x.OrderStatus != OrderStatus.Pending &&
                                     x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());

            var nakitSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
            && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Nakit")
             .Select(x => x.Order.Where(x =>
                                  x.OrderStatus != OrderStatus.Cancel &&
                                  x.OrderStatus != OrderStatus.Pending &&
                                  x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());



            var KrediKartıSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
            && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Kredi Kartı")
             .Select(x => x.Order.Where(x =>
                                  x.OrderStatus != OrderStatus.Cancel &&
                                  x.OrderStatus != OrderStatus.Pending &&
                                  x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var MultinetSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
           && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Multinet")
            .Select(x => x.Order.Where(x =>
                                 x.OrderStatus != OrderStatus.Cancel &&
                                 x.OrderStatus != OrderStatus.Pending &&
                                 x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());

            var SodexoSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
          && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Sodexo")
           .Select(x => x.Order.Where(x =>
                                x.OrderStatus != OrderStatus.Cancel &&
                                x.OrderStatus != OrderStatus.Pending &&
                                x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var TicketRestaurantSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
          && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Ticket Restaurant")
           .Select(x => x.Order.Where(x =>
                                x.OrderStatus != OrderStatus.Cancel &&
                                x.OrderStatus != OrderStatus.Pending &&
                                x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var SetCardSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
         && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "SetCard")
          .Select(x => x.Order.Where(x =>
                               x.OrderStatus != OrderStatus.Cancel &&
                               x.OrderStatus != OrderStatus.Pending &&
                               x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var WinwinSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
         && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Winwin")
          .Select(x => x.Order.Where(x =>
                               x.OrderStatus != OrderStatus.Cancel &&
                               x.OrderStatus != OrderStatus.Pending &&
                               x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var MetropolSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.PaymentCompleted && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt
         && x.OrderPayment.VenuePaymentMethod.PaymentMethod.Text == "Metropol")
          .Select(x => x.Order.Where(x =>
                               x.OrderStatus != OrderStatus.Cancel &&
                               x.OrderStatus != OrderStatus.Pending &&
                               x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());
            var KacakSum = string.Format("{0:N2}", orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.NoPayment && x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt)
         .Select(x => x.Order.Where(x =>
                              x.OrderStatus != OrderStatus.Cancel &&
                              x.OrderStatus != OrderStatus.Pending &&
                              x.OrderStatus != OrderStatus.Denied).Select(y => y.OrderDetail.Sum(or => or.Price * or.Quantity)).Sum()).Sum());




            var KacakSayisi = orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.NoPayment &&
                                    x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt).Count();

            var VipSayisi = orderTables.Where(x => x.OrderCash.OrderCashStatus == OrderCashStatus.Treat &&
                                    x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt).Count();

            var Toplammusteri = orderTables.Where(x =>
                                    x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt).Count();

            var onlineMusteri = orderTables.Where(x => x.User.IsGuest == false &&
                                    x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt).Count();

            var normalMusteri = orderTables.Where(x => x.User.IsGuest == true &&
                                    x.OrderCash.CreatedDate > dt && x.OrderCash.CreatedDate < endDt).Count();

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

            var orderTables = _orderTableService.GetByTableId(cash.Venue.Id, tableId, false)
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