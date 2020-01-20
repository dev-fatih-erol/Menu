using System;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Extensions;
using Menu.Api.Helpers;
using Menu.Api.Models;
using Menu.Core.Enums;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [Route("Me/Order/CheckOut")]
        public IActionResult CheckOut(int usedPoint, int tip)
        {
            var orderTable = _orderTableService.GetByUserId(User.Identity.GetId(), false);

            if (orderTable != null)
            {
                if (orderTable.OrderPayment != null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Mevcut hesap ödeme isteğiniz var. Hesap isteyemezsiniz."
                    });
                }

                var pendingOrders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending).ToList();

                if (pendingOrders.Any())
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Bekleyen bir siparişiniz var. Hesap isteyemezsiniz."
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

        // Get me/orders
        [HttpGet]
        [Authorize]
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
        [Authorize]
        [Route("Order")]
        public IActionResult Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = ModelState.GetErrors()
                });
            }

            var table = _tableService.GetById(dto.TableId, dto.VenueId);

            if (table != null)
            {
                var orderTable = _orderTableService.GetByUserId(User.Identity.GetId(), false);

                if (orderTable != null)
                {
                    if (!orderTable.TableId.Equals(dto.TableId) || !orderTable.VenueId.Equals(dto.VenueId))
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Başka bir mekandan veya masadan sipariş veremezsiniz."
                        });
                    }

                    if (orderTable.OrderPayment != null)
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Hesap isteme işleminden sonra sipariş veremezsiniz."
                        });
                    }

                    var pendingOrders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending).ToList();

                    if (pendingOrders.Count() > 5)
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Çok sayıda bekleyen siparişiniz var."
                        });
                    }

                    //Repeat Order

                    var order = new Order
                    {
                        Code = RandomHelper.Generate(1000, 9999).ToString(),
                        Description = dto.Description ?? null,
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

                    return Ok(new
                    {
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = true
                    });
                }

                //New Order

                var newOrderTable = new OrderTable
                {
                    IsClosed = false,
                    CreatedDate = DateTime.Now,
                    VenueId = dto.VenueId,
                    TableId = dto.TableId,
                    UserId = User.Identity.GetId()
                };

                var newOrder = new Order
                {
                    Code = RandomHelper.Generate(1000, 9999).ToString(),
                    Description = dto.Description ?? null,
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
                Message = "Masa bulunamadı"
            });
        }
    }
}