using System;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IMapper _mapper;

        private readonly IOrderTableService _orderTableService;

        private readonly IOrderService _orderService;

        private readonly IOrderDetailService _orderDetailService;

        private readonly IOrderPaymentService _orderPaymentService;

        private readonly IVenueService _venueService;

        private readonly ITableService _tableService;

        private readonly IWaiterService _waiterService;

        private readonly IUserService _userService;

        private readonly IProductService _productService;

        private readonly IOptionItemService _optionItemService;

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderTableService orderTableService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IOrderPaymentService orderPaymentService,
            IVenueService venueService,
            ITableService tableService,
            IWaiterService waiterService,
            IUserService userService,
            IProductService productService,
            IOptionItemService optionItemService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderTableService = orderTableService;

            _orderService = orderService;

            _orderDetailService = orderDetailService;

            _orderPaymentService = orderPaymentService;

            _venueService = venueService;

            _tableService = tableService;

            _waiterService = waiterService;

            _userService = userService;

            _productService = productService;

            _optionItemService = optionItemService;
        }

        // Post me/order/checkout
        [HttpPost]
        [Route("Me/Order/CheckOut")]
        public IActionResult CheckOut(int usedPoint, int tip)
        {
            var orderTable = _orderTableService.GetByUserId(10, false);

            if (orderTable != null)
            {
                if (orderTable.OrderPayment != null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Zaten hesap ödeme isteğiniz var."
                    });
                }

                var pendingOrders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending).ToList();

                if (pendingOrders.Any())
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Bekleyen bir siparişiniz var"
                    });
                }

                var totalPrice = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                             o.OrderStatus != OrderStatus.Denied)
                                                 .Select(o => o.OrderDetail
                                                 .Sum(o => o.Price * o.Quantity)).Sum();

                var newOrderPayment = new OrderPayment
                {
                    VenuePaymentMethodId = 1,
                    Tip = tip,
                    EarnedPoint = Convert.ToInt32(totalPrice) * 10,
                    UsedPoint = usedPoint,
                    CreatedDate = DateTime.Now,
                    OrderTableId = orderTable.Id
                };

                _orderPaymentService.Create(newOrderPayment);

                _orderPaymentService.SaveChanges();


                var user = _userService.GetById(10);

                user.Point = (user.Point - usedPoint) + (Convert.ToInt32(totalPrice) * 10);

                _userService.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = user
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }

        // Get me/orders
        [HttpGet]
        [Route("Me/Orders")]
        public IActionResult GetByUserId()
        {
            var orderTables = _orderTableService.GetByUserId(10);

            if (orderTables.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = orderTables
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }

        // POST order
        [HttpPost]
        [Route("Order")]
        public IActionResult Create([FromBody] CreateOrderDto dto)
        {
            var orderTable = _orderTableService.GetByUserId(9, false);

            if (orderTable != null)
            {
                if (!orderTable.TableId.Equals(dto.TableId) || !orderTable.VenueId.Equals(dto.VenueId))
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Başka bir cafeden veya masadan sipariş veremezsiniz"
                    });
                }

                if (orderTable.OrderPayment != null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Hesap istediniz sipariş veremezsiniz"
                    });
                }

                var pendingOrders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending).ToList();

                if (pendingOrders.Count() > 5)
                {
                    return NotFound(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = "Çok sayıda siparişiniz var lütfen diğer siparişlerinizin onaylanmasını bekleyin."
                    });
                }

                //Repeat Order

                var order = new Order
                {
                    Code = Guid.NewGuid().ToString(),
                    Description = dto.Description,
                    OrderStatus = OrderStatus.Pending,
                    CreatedDate = DateTime.Now,
                    OrderTableId = orderTable.Id
                };

                foreach (var orderDetail in dto.OrderDetail)
                {
                    var product = _productService.GetById(orderDetail.ProductId);

                    if (product != null)
                    {
                        string optionItemText = null;

                        foreach (var item in orderDetail.OptionItems)
                        {
                            var optionItem = _optionItemService.GetById(item);

                            if (optionItem != null)
                            {
                                optionItemText = optionItem.Name + ',';
                            }
                        }

                        var newOrderDetail = new OrderDetail
                        {
                            Name = product.Name,
                            Photo = product.Photo,
                            OptionItem = optionItemText.TrimEnd(','),
                            Quantity = orderDetail.Quantity,
                            Price = product.Price,
                            Order = order
                        };

                        _orderDetailService.Create(newOrderDetail);
                    }
                }

                _orderService.Create(order);

                _orderService.SaveChanges();

                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Ürün bulunamadı"
                });
            }

            //New Order

            var newOrderTable = new OrderTable
            {
                IsClosed = false,
                CreatedDate = DateTime.Now,
                VenueId = dto.VenueId,
                TableId = dto.TableId,
                UserId = 9
            };

            var newOrder = new Order
            {
                Code = Guid.NewGuid().ToString(),
                Description = dto.Description,
                OrderStatus = OrderStatus.Pending,
                CreatedDate = DateTime.Now,
                OrderTable = newOrderTable
            };

            foreach (var orderDetail in dto.OrderDetail)
            {
                var product = _productService.GetById(orderDetail.ProductId);

                if (product != null)
                {
                    string optionItemText = null;

                    foreach (var item in orderDetail.OptionItems)
                    {
                        var optionItem = _optionItemService.GetById(item);

                        if (optionItem != null)
                        {
                            optionItemText = optionItem.Name + ',';
                        }
                    }

                    var newOrderDetail = new OrderDetail
                    {
                        Name = product.Name,
                        Photo = product.Photo,
                        OptionItem = optionItemText.TrimEnd(','),
                        Quantity = orderDetail.Quantity,
                        Price = product.Price,
                        Order = newOrder
                    };

                    _orderDetailService.Create(newOrderDetail);
                }
            }

            _orderTableService.Create(newOrderTable);

            _orderTableService.SaveChanges();

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }
    }
}