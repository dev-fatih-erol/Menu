using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        private readonly ITableWaiterService _tableWaiterService;

        private readonly ITableService _tableService;

        private readonly IUserTokenService _userTokenService;

        private readonly string _key = "key=AAAA7Tr-w-A:APA91bFkdAPrjKgsrKdzqFpR1EXzmie3oUk6KaVgaPmdCyNdOsik_zyMJZHo2MgAAXYShzwJjj1dnlPpn-DvhW5JnYyzwDyahdVV9FyoHYV4K6XUggKJTm0uXRLxVhodorwKEzThBkqc";

        public DashboardController(IOrderService orderService,
            IKitchenService kitchenService,
            ITableWaiterService tableWaiterService,
            ITableService tableService,
            IUserTokenService userTokenService)
        {
            _orderService = orderService;

            _kitchenService = kitchenService;

            _tableWaiterService = tableWaiterService;

            _tableService = tableService;

            _userTokenService = userTokenService;
        }

        [HttpPost]
        [Authorize]
        [Route("Order/{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatus orderStatus, int tableId, int orderTableId, int userId)
        {
            var order = _orderService.GetById(id);

            if (order != null)
            {
                if (orderStatus == OrderStatus.Prepared)
                {
                    var waiters = _tableWaiterService.GetByTableId(tableId);

                    var tokens = waiters.Select(s => s.Waiter.WaiterToken.Token).ToList();

                    if (tokens.Count() > 0 || tokens != null)
                    {
                        var table = _tableService.GetById(tableId);

                        dynamic foo = new ExpandoObject();
                        foo.registration_ids = tokens;
                        foo.notification = new
                        {
                            title = "Sipariş Durumu",
                            body = table.Name + " isimli masanın siparişi hazır, Mutfaktan gelip alabilirsiniz.",
                            data = new { table.Id }
                        };

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                        using var httpClient = new HttpClient();

                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                        var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                        await response.Content.ReadAsStringAsync();
                    }
                }
                else if (orderStatus == OrderStatus.Closed)
                {

                    var userToken1 = _userTokenService.GetByUserId(userId);

                    if (userToken1 != null)
                    {
                        string[] test = new string[1];
                        test[0] = userToken1.Token;
                        dynamic foo = new ExpandoObject();
                        foo.registration_ids = test;
                        foo.notification = new
                        {
                            title = "Sipariş Durumu",
                            body = "Siparişiniz geliyor, Afiyet olsun.",
                            data = new { orderTableId}
                        };

                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                        using var httpClient = new HttpClient();

                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                        var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                        await response.Content.ReadAsStringAsync();
                    }
                }

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
                        OrderStatus = order.OrderStatus.ToOrderStatus(),
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
                            order.OrderTable.Table.Id,
                            order.OrderTable.Table.Name,
                        },
                        User = new
                        {
                            order.OrderTable.User.Id,
                            order.OrderTable.User.Name,
                            order.OrderTable.User.Surname,
                            order.OrderTable.User.Photo
                        },
                        Waiter = new
                        {
                            order.OrderWaiter.Waiter.Name,
                            order.OrderWaiter.Waiter.Surname
                        },
                        OrderTable = new {
                            order.OrderTable.Id
                        }
                    }));
                }

                return NotFound("Sipariş tipi bulunamadı");
            }

            return NotFound("Mutfak bulunamadı");
        }
    }
}