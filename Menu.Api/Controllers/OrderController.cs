using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        private readonly IUserService _userService;

        private readonly IProductService _productService;

        private readonly IOptionService _optionService;

        private readonly IOptionItemService _optionItemService;

        private readonly ITableWaiterService _tableWaiterService;

        private readonly IOrderWaiterService _orderWaiterService;

        private readonly ICashService _cashService;

        private readonly IVenuePaymentMethodService _venuePaymentMethodService;

        private readonly string _key = "key=AAAA7Tr-w-A:APA91bFkdAPrjKgsrKdzqFpR1EXzmie3oUk6KaVgaPmdCyNdOsik_zyMJZHo2MgAAXYShzwJjj1dnlPpn-DvhW5JnYyzwDyahdVV9FyoHYV4K6XUggKJTm0uXRLxVhodorwKEzThBkqc";

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderTableService orderTableService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IOrderPaymentService orderPaymentService,
            IVenueService venueService,
            ITableService tableService,
            IUserService userService,
            IProductService productService,
            IOptionService optionService,
            IOptionItemService optionItemService,
            ITableWaiterService tableWaiterService,
            IOrderWaiterService orderWaiterService,
            IVenuePaymentMethodService venuePaymentMethodService,
            ICashService cashService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderTableService = orderTableService;

            _orderService = orderService;

            _orderDetailService = orderDetailService;

            _orderPaymentService = orderPaymentService;

            _venueService = venueService;

            _tableService = tableService;

            _userService = userService;

            _productService = productService;

            _optionService = optionService;

            _optionItemService = optionItemService;

            _tableWaiterService = tableWaiterService;

            _orderWaiterService = orderWaiterService;

            _venuePaymentMethodService = venuePaymentMethodService;

            _cashService = cashService;
        }

        // Get waiter/table/5/order/checkout
        [HttpGet]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Table/{tableId:int}/Order/CheckOut")]
        public IActionResult WaiterCheckOut(int tableId)
        {
            var orderPayments = _orderPaymentService.GetByIsClosedAndTableId(tableId, false);

            if (orderPayments.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = orderPayments.Select(orderPayment => new {
                        orderPayment.Id,
                        orderPayment.Tip,
                        TLPoint = orderPayment.EarnedPoint * 0.001,
                        orderPayment.CreatedDate,
                        orderPayment.OrderTable.User.Name,
                        orderPayment.OrderTable.User.Surname,
                        orderPayment.VenuePaymentMethod.PaymentMethod.Text,
                        OrderTotalPrice = string.Format("{0:N2}", orderPayment.OrderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                                                                      o.OrderStatus != OrderStatus.Denied)
                                                     .Select(or => or.OrderDetail
                                                     .Sum(or => or.Price * or.Quantity)).Sum()),
                        TotalPrice = string.Format("{0:N2}", Convert.ToInt32(orderPayment.OrderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                                                                      o.OrderStatus != OrderStatus.Denied)
                                                     .Select(or => or.OrderDetail
                                                     .Sum(or => or.Price * or.Quantity)).Sum() + orderPayment.Tip) - (orderPayment.EarnedPoint * 0.001))
                    })
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Hesap isteği bulunamadı"
            });
        }

        // POST Waiter/Order/5/ChangeStatus/1
        [HttpPost]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Order/{id:int}/ChangeStatus/{orderStatus:range(1,2)}")]
        public IActionResult ChangeOrderStatus(int id, OrderStatus orderStatus)
        {
            var order = _orderService.GetById(id, OrderStatus.Pending);

            if (order != null)
            {
                order.OrderStatus = orderStatus;

                _orderService.SaveChanges();

                var newOrderWaiter = new OrderWaiter
                {
                    OrderId = order.Id,
                    WaiterId = User.Identity.GetId(),
                    CreatedDate = DateTime.Now
                };

                _orderWaiterService.Create(newOrderWaiter);

                _orderWaiterService.SaveChanges();

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
                Message = "Sipariş Bulunamadı"
            });
        }

        // Get Waiter/Table/5/Order
        [HttpGet]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Table/{tableId:int}/Order/Pending")]
        public IActionResult GetPendingByTableId(int tableId)
        {
            var tableWaiter = _tableWaiterService.GetByTableIdAndWaiterId(tableId, User.Identity.GetId());

            if (tableWaiter == null)
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Garson veya masa bulunamadı"
                });
            }

            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByTableId(cash.Venue.Id, tableId, false)
                                           .Where(o => o.Order.Any(o => o.OrderStatus == OrderStatus.Pending)).ToList();

            if (!orderTables.Any())
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Sipariş bulunamadı"
                });
            }

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = orderTables.Select(orderTable => new
                {
                    orderTable.Id,
                    orderTable.IsClosed,
                    orderTable.CreatedDate,
                    orderTable.User.Name,
                    orderTable.User.Surname,
                    Orders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending).Select(o => new
                    {
                        o.Id,
                        o.Code,
                        o.Description,
                        o.OrderStatus,
                        OrderDetail = o.OrderDetail.Select(o => new
                        {
                            o.Id,
                            o.Name,
                            o.Photo,
                            o.Quantity,
                            o.Price,
                            o.OptionItem,
                        }).ToList(),
                        TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(or => or.Id == o.Id &&
                                                                  or.OrderStatus == OrderStatus.Pending)
                                                     .Select(or => or.OrderDetail
                                                     .Sum(or => or.Price * or.Quantity)).Sum())
                    }).ToList(),
                    TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Pending)
                                                     .Select(o => o.OrderDetail
                                                     .Sum(o => o.Price * o.Quantity)).Sum())
                })
            });
        }

        // Get Waiter/Table/5/Order
        [HttpGet]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Table/{tableId:int}/Order")]
        public IActionResult GetByTableId(int tableId)
        {
            var tableWaiter = _tableWaiterService.GetByTableIdAndWaiterId(tableId, User.Identity.GetId());

            if (tableWaiter == null)
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Garson veya masa bulunamadı"
                });
            }

            var cash = _cashService.GetById(User.Identity.GetId());

            var orderTables = _orderTableService.GetByTableId(cash.Venue.Id, tableId, false)
                                           .Where(o => o.Order.Any(o => o.OrderStatus != OrderStatus.Pending)).ToList();

            if (!orderTables.Any())
            {
                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Sipariş bulunamadı"
                });
            }

            return Ok(new
            {
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = orderTables.Select(orderTable => new
                {
                    orderTable.Id,
                    orderTable.IsClosed,
                    orderTable.CreatedDate,
                    orderTable.User.Name,
                    orderTable.User.Surname,
                    Orders = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Pending).Select(o => new
                    {
                        o.Id,
                        o.Code,
                        o.Description,
                        o.OrderStatus,
                        o.OrderWaiter.Waiter.Name,
                        o.OrderWaiter.Waiter.Surname,
                        OrderDetail = o.OrderDetail.Select(o => new
                        {
                            o.Id,
                            o.Name,
                            o.Photo,
                            o.Quantity,
                            o.Price,
                            o.OptionItem,
                        }).ToList(),
                        TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(or => or.Id == o.Id &&
                                                                  or.OrderStatus != OrderStatus.Cancel &&
                                                                  or.OrderStatus != OrderStatus.Denied)
                                                     .Select(or => or.OrderDetail
                                                     .Sum(or => or.Price * or.Quantity)).Sum())
                    }).ToList(),
                    TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                             o.OrderStatus != OrderStatus.Denied)
                                                     .Select(o => o.OrderDetail
                                                     .Sum(o => o.Price * o.Quantity)).Sum())
                })
            });
        }

        // Get me/order/checkout
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Me/Order/CheckOut")]
        public IActionResult UserCheckOut()
        {
            var orderTable = _orderTableService.GetByUserId(User.Identity.GetId(), false);

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

                var user = _userService.GetById(User.Identity.GetId());

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
                        user.Point,
                        TLPoint = user.Point * 0.001,
                        TotalPrice = string.Format("{0:N2}", totalPrice)
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

        // Post me/order/checkout
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("Me/Order/CheckOut")]
        public async Task<IActionResult> CheckOut(CreateCheckOutDto dto)
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

                var venuePaymentMethod = _venuePaymentMethodService.GetByVenueId(dto.PaymentMethodId, orderTable.VenueId);

                if (venuePaymentMethod == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Seçmiş olduğunuz ödeme tipi bu mekana ait değil"
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

                var closedOrders = orderTable.Order.Where(o => o.OrderStatus == OrderStatus.Closed).ToList();

                if (!closedOrders.Any())
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Teslim edilmiş bir siparişiniz olmadığı için hesap isteyemezsiniz"
                    });
                }


                var point = _userService.GetById(User.Identity.GetId()).Point;

                if (point < dto.UsedPoint)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Seçtiğiniz puan, Sahip olduğunuz puan sınırını aşıyor."
                    });
                }

                var totalPrice = orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                             o.OrderStatus != OrderStatus.Denied)
                                 .Select(o => o.OrderDetail
                                 .Sum(o => o.Price * o.Quantity)).Sum();

                if (Convert.ToInt32(totalPrice) < dto.UsedPoint * 0.001)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Sipariş toplamınızdan daha düşük puan kullanmalısınız."
                    });
                }

                var newOrderPayment = new OrderPayment
                {
                    VenuePaymentMethodId = venuePaymentMethod.Id,
                    Tip = dto.Tip,
                    EarnedPoint = Convert.ToInt32(totalPrice) * 10,
                    UsedPoint = dto.UsedPoint,
                    CreatedDate = DateTime.Now,
                    OrderTableId = orderTable.Id
                };

                _orderPaymentService.Create(newOrderPayment);

                _orderPaymentService.SaveChanges();

                var waiters = _tableWaiterService.GetByTableId(orderTable.TableId);

                var tokens = waiters.Select(s => s.Waiter.WaiterToken.Token).ToList();

                if (tokens.Count() > 0 || tokens != null)
                {
                    dynamic foo = new ExpandoObject();
                    foo.registration_ids = tokens;
                    foo.notification = new
                    {
                        body = waiters.Select(x => x.Table.Name).FirstOrDefault() + " isimli masa " + venuePaymentMethod.PaymentMethod.Text + " ile hesap istemiştir"
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

        // Get me/order/5/details
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Me/Order/{id:int}/Details")]
        public IActionResult OrderDetails(int id, bool status = true)
        {
            OrderTable orderTable = null;

            if (status)
            {
                orderTable = _orderTableService.GetDetailById(id, User.Identity.GetId());

                if (orderTable != null)
                {
                    return Ok(new
                    {
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = new
                        {
                            orderTable.Id,
                            orderTable.IsClosed,
                            orderTable.CreatedDate,
                            Table = new
                            {
                                orderTable.Table.Id,
                                orderTable.Table.Name
                            },
                            Venue = new
                            {
                                orderTable.Venue.Id,
                                orderTable.Venue.Name,
                                orderTable.Venue.Photo
                            },
                            Order = orderTable.Order.OrderByDescending(o => o.CreatedDate).Select(o => new
                            {
                                o.Id,
                                o.Code,
                                o.Description,
                                o.OrderStatus,
                                OrderDetail = o.OrderDetail.Select(o => new
                                {
                                    o.Id,
                                    o.Name,
                                    o.Photo,
                                    o.Quantity,
                                    o.Price,
                                    o.OptionItem,
                                }).ToList(),
                                TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(or => or.Id == o.Id &&
                                                                          or.OrderStatus != OrderStatus.Cancel &&
                                                                          or.OrderStatus != OrderStatus.Denied)
                                                             .Select(or => or.OrderDetail
                                                             .Sum(or => or.Price * or.Quantity)).Sum())
                            }).ToList(),
                            OrderPayment = new
                            {
                                orderTable.OrderPayment.Tip,
                                orderTable.OrderPayment.EarnedPoint,
                                orderTable.OrderPayment.UsedPoint,
                                orderTable.OrderPayment.CreatedDate,
                                orderTable.OrderPayment.VenuePaymentMethod.PaymentMethod.Text
                            },
                            TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                                     o.OrderStatus != OrderStatus.Denied)
                                                         .Select(o => o.OrderDetail
                                                         .Sum(o => o.Price * o.Quantity)).Sum())
                        }
                    });
                }

                return NotFound(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Sipariş bulanamadı"
                });
            }

            orderTable = _orderTableService.GetDetailById(id, User.Identity.GetId(), false);

            if (orderTable != null)
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new
                    {
                        orderTable.Id,
                        orderTable.IsClosed,
                        orderTable.CreatedDate,
                        Table = new
                        {
                            orderTable.Table.Id,
                            orderTable.Table.Name
                        },
                        Venue = new
                        {
                            orderTable.Venue.Id,
                            orderTable.Venue.Name,
                            orderTable.Venue.Photo
                        },
                        Order = orderTable.Order.OrderByDescending(o => o.CreatedDate).Select(o => new
                        {
                            o.Id,
                            o.Code,
                            o.Description,
                            o.OrderStatus,
                            OrderDetail = o.OrderDetail.Select(o => new
                            {
                                o.Id,
                                o.Name,
                                o.Photo,
                                o.Quantity,
                                o.Price,
                                o.OptionItem,
                            }).ToList(),
                            TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(or => or.Id == o.Id &&
                                                                      or.OrderStatus != OrderStatus.Cancel &&
                                                                      or.OrderStatus != OrderStatus.Denied &&
                                                                      o.OrderStatus != OrderStatus.Pending)
                                                         .Select(or => or.OrderDetail
                                                         .Sum(or => or.Price * or.Quantity)).Sum())
                        }).ToList(),
                        TotalPrice = string.Format("{0:N2}", orderTable.Order.Where(o => o.OrderStatus != OrderStatus.Cancel &&
                                                                 o.OrderStatus != OrderStatus.Denied &&
                                                                 o.OrderStatus != OrderStatus.Pending)
                                                     .Select(o => o.OrderDetail
                                                     .Sum(o => o.Price * o.Quantity)).Sum())
                    }
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Sipariş bulanamadı"
            });
        }

        // Get me/orders
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("Me/Orders")]
        public IActionResult GetByUserId()
        {
            var orderTables = _orderTableService.GetByUserId(User.Identity.GetId());

            if (orderTables.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = orderTables.Select(o => new
                    {
                        o.Id,
                        o.IsClosed,
                        o.CreatedDate,
                        o.Venue.Name
                    }).ToList()
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Sipariş bulanamadı"
            });
        }

        [HttpPost]
        [Authorize(Roles = "Waiter")]
        [Route("Waiter/Order/Venue/{venueId:int}/Table/{tableId:int}/Guest/{guestId:int}")]
        public IActionResult Create(int venueId, int tableId,int guestId, [FromBody] CreateOrderDto dto)
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

            var table = _tableService.GetById(tableId, venueId);

            if (table != null)
            {
                var orderTable = _orderTableService.GetByUserId(guestId, false);

                if (orderTable != null)
                {
                    if (!orderTable.TableId.Equals(tableId) || !orderTable.VenueId.Equals(venueId))
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

                    var order = new Order
                    {
                        Code = RandomHelper.Generate(1000, 9999).ToString(),
                        Description = dto.Description ?? null,
                        OrderStatus = OrderStatus.Approved,
                        CreatedDate = DateTime.Now,
                        OrderTableId = orderTable.Id
                    };

                    foreach (var orderDetail in dto.OrderDetail)
                    {
                        var product = _productService.GetByIdAndVenueId(orderDetail.ProductId, venueId);

                        if (product == null)
                        {
                            return NotFound(new
                            {
                                Success = false,
                                StatusCode = (int)HttpStatusCode.NotFound,
                                Message = "Başka bir mekanda ürünler mevcut"
                            });
                        }

                        var openingTime = new TimeSpan(product.OpeningTime);

                        var closingTime = new TimeSpan(product.ClosingTime);

                        var currentTime = DateTime.Now.TimeOfDay;

                        if (!((currentTime >= openingTime) && (currentTime <= closingTime)))
                        {
                            return BadRequest(new
                            {
                                Success = false,
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Message = $"{product.Name} ürünü şuan mevcut değil"
                            });
                        }

                        string optionItemText = null;

                        var productPrice = product.Price;

                        if (orderDetail.Options != null && orderDetail.Options.Any())
                        {
                            foreach (var orderOption in orderDetail.Options)
                            {
                                var option = _optionService.GetById(orderOption.Id);

                                if (option != null)
                                {
                                    foreach (var orderOptionItem in orderOption.OptionItems)
                                    {
                                        var optionItem = _optionItemService.GetById(orderOptionItem.Id, option.Id);

                                        if (optionItem != null)
                                        {
                                            optionItemText += optionItem.Name + ',';

                                            productPrice += optionItem.Price;
                                        }
                                    }
                                }
                            }

                            optionItemText = optionItemText.TrimEnd(',');
                        }

                        var newOrderDetail = new OrderDetail
                        {
                            Name = product.Name,
                            Photo = product.Photo,
                            OptionItem = optionItemText,
                            Quantity = orderDetail.Quantity,
                            Price = productPrice,
                            Order = order
                        };

                        _orderDetailService.Create(newOrderDetail);
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

                var newOrderTable = new OrderTable
                {
                    IsClosed = false,
                    CreatedDate = DateTime.Now,
                    VenueId = venueId,
                    TableId = tableId,
                    UserId = guestId
                };

                var newOrder = new Order
                {
                    Code = RandomHelper.Generate(1000, 9999).ToString(),
                    Description = dto.Description ?? null,
                    OrderStatus = OrderStatus.Approved,
                    CreatedDate = DateTime.Now,
                    OrderTable = newOrderTable
                };

                foreach (var orderDetail in dto.OrderDetail)
                {
                    var product = _productService.GetByIdAndVenueId(orderDetail.ProductId, venueId);

                    if (product == null)
                    {
                        return NotFound(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Message = "Başka bir mekana ürünler mevcut"
                        });
                    }

                    var openingTime = new TimeSpan(product.OpeningTime);

                    var closingTime = new TimeSpan(product.ClosingTime);

                    var currentTime = DateTime.Now.TimeOfDay;

                    if (!((currentTime >= openingTime) && (currentTime <= closingTime)))
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = $"{product.Name} ürünü şuan mevcut değil"
                        });
                    }

                    string optionItemText = null;

                    var productPrice = product.Price;

                    if (orderDetail.Options != null && orderDetail.Options.Any())
                    {
                        foreach (var orderOption in orderDetail.Options)
                        {
                            var option = _optionService.GetById(orderOption.Id);

                            if (option != null)
                            {
                                foreach (var orderOptionItem in orderOption.OptionItems)
                                {
                                    var optionItem = _optionItemService.GetById(orderOptionItem.Id, option.Id);

                                    if (optionItem != null)
                                    {
                                        optionItemText += optionItem.Name + ',';

                                        productPrice += optionItem.Price;
                                    }
                                }
                            }
                        }

                        optionItemText = optionItemText.TrimEnd(',');
                    }

                    var newOrderDetail = new OrderDetail
                    {
                        Name = product.Name,
                        Photo = product.Photo,
                        OptionItem = optionItemText,
                        Quantity = orderDetail.Quantity,
                        Price = productPrice,
                        Order = newOrder
                    };

                    _orderDetailService.Create(newOrderDetail);
                }

                _orderTableService.Create(newOrderTable);

                _orderTableService.SaveChanges();

                var changedTableStatus = _tableService.GetById(tableId);

                if (changedTableStatus.TableStatus == TableStatus.Closed)
                {
                    changedTableStatus.TableStatus = TableStatus.Open;

                    _tableService.SaveChanges();
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
                Message = "Mekan veya masa bulunamadı"
            });
        }

        // POST user/order/venue/5/table/5
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("User/Order/Venue/{venueId:int}/Table/{tableId:int}")]
        public async Task<IActionResult> Create(int venueId, int tableId, [FromBody] CreateOrderDto dto)
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

            var table = _tableService.GetById(tableId, venueId);

            if (table != null)
            {
                var orderTable = _orderTableService.GetByUserId(User.Identity.GetId(), false);

                if (orderTable != null)
                {
                    if (!orderTable.TableId.Equals(tableId) || !orderTable.VenueId.Equals(venueId))
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

                    if (pendingOrders.Count() > 3)
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Çok sayıda bekleyen siparişiniz var."
                        });
                    }

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
                        var product = _productService.GetByIdAndVenueId(orderDetail.ProductId, venueId);

                        if (product == null)
                        {
                            return NotFound(new
                            {
                                Success = false,
                                StatusCode = (int)HttpStatusCode.NotFound,
                                Message = "Başka bir mekanda ürünler mevcut"
                            });
                        }

                        var openingTime = new TimeSpan(product.OpeningTime);

                        var closingTime = new TimeSpan(product.ClosingTime);

                        var currentTime = DateTime.Now.TimeOfDay;

                        if (!((currentTime >= openingTime) && (currentTime <= closingTime)))
                        {
                            return BadRequest(new
                            {
                                Success = false,
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Message = $"{product.Name} ürünü şuan mevcut değil"
                            });
                        }

                        string optionItemText = null;

                        var productPrice = product.Price;

                        if (orderDetail.Options != null && orderDetail.Options.Any())
                        {
                            foreach (var orderOption in orderDetail.Options)
                            {
                                var option = _optionService.GetById(orderOption.Id);

                                if (option != null)
                                {
                                    foreach (var orderOptionItem in orderOption.OptionItems)
                                    {
                                        var optionItem = _optionItemService.GetById(orderOptionItem.Id, option.Id);

                                        if (optionItem != null)
                                        {
                                            optionItemText += optionItem.Name + ',';

                                            productPrice += optionItem.Price;
                                        }
                                    }
                                }
                            }

                            optionItemText = optionItemText.TrimEnd(',');
                        }

                        var newOrderDetail = new OrderDetail
                        {
                            Name = product.Name,
                            Photo = product.Photo,
                            OptionItem = optionItemText,
                            Quantity = orderDetail.Quantity,
                            Price = productPrice,
                            Order = order
                        };

                        _orderDetailService.Create(newOrderDetail);
                    }

                    _orderService.Create(order);

                    _orderService.SaveChanges();

                    var waiters1 = _tableWaiterService.GetByTableId(tableId);

                    var tokens1 = waiters1.Select(s => s.Waiter.WaiterToken.Token).ToList();

                    if (tokens1.Count() > 0 || tokens1 != null)
                    {
                        dynamic foo = new ExpandoObject();
                        foo.registration_ids = tokens1;
                        foo.notification = new
                        {
                            body = table.Name + " isimli masadan sipariş gelmiştir"
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

                var newOrderTable = new OrderTable
                {
                    IsClosed = false,
                    CreatedDate = DateTime.Now,
                    VenueId = venueId,
                    TableId = tableId,
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
                    var product = _productService.GetByIdAndVenueId(orderDetail.ProductId, venueId);

                    if (product == null)
                    {
                        return NotFound(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Message = "Başka bir mekana ürünler mevcut"
                        });
                    }

                    var openingTime = new TimeSpan(product.OpeningTime);

                    var closingTime = new TimeSpan(product.ClosingTime);

                    var currentTime = DateTime.Now.TimeOfDay;

                    if (!((currentTime >= openingTime) && (currentTime <= closingTime)))
                    {
                        return BadRequest(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = $"{product.Name} ürünü şuan mevcut değil"
                        });
                    }

                    string optionItemText = null;

                    var productPrice = product.Price;

                    if (orderDetail.Options != null && orderDetail.Options.Any())
                    {
                        foreach (var orderOption in orderDetail.Options)
                        {
                            var option = _optionService.GetById(orderOption.Id);

                            if (option != null)
                            {
                                foreach (var orderOptionItem in orderOption.OptionItems)
                                {
                                    var optionItem = _optionItemService.GetById(orderOptionItem.Id, option.Id);

                                    if (optionItem != null)
                                    {
                                        optionItemText += optionItem.Name + ',';

                                        productPrice += optionItem.Price;
                                    }
                                }
                            }
                        }

                        optionItemText = optionItemText.TrimEnd(',');
                    }

                    var newOrderDetail = new OrderDetail
                    {
                        Name = product.Name,
                        Photo = product.Photo,
                        OptionItem = optionItemText,
                        Quantity = orderDetail.Quantity,
                        Price = productPrice,
                        Order = newOrder
                    };

                    _orderDetailService.Create(newOrderDetail);
                }

                _orderTableService.Create(newOrderTable);

                _orderTableService.SaveChanges();

                var changedTableStatus = _tableService.GetById(tableId);

                if (changedTableStatus.TableStatus == TableStatus.Closed)
                {
                    changedTableStatus.TableStatus = TableStatus.Open;

                    _tableService.SaveChanges();
                }

                var waiters = _tableWaiterService.GetByTableId(tableId);

                var tokens = waiters.Select(s => s.Waiter.WaiterToken.Token).ToList();

                if (tokens.Count() > 0 || tokens != null)
                {
                    dynamic foo = new ExpandoObject();
                    foo.registration_ids = tokens;
                    foo.notification = new
                    {
                        body = table.Name + " isimli masadan sipariş gelmiştir"
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
                Message = "Mekan veya masa bulunamadı"
            });
        }
    }
}