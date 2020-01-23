﻿using System;
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

        private readonly IOptionService _optionService;

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
            IOptionService optionService,
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

            _optionService = optionService;

            _optionItemService = optionItemService;
        }

        // Get me/order/checkout
        [HttpGet]
        [Authorize]
        [Route("Me/Order/CheckOut")]
        public IActionResult CheckOut()
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
        [Authorize]
        [Route("Me/Order/CheckOut")]
        public IActionResult CheckOut(int usedPoint, int tip, int venuePaymentMethodId)
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

                var point = _userService.GetById(User.Identity.GetId()).Point;

                if (point < usedPoint)
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

                if (Convert.ToInt32(totalPrice) < usedPoint * 0.001)
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
                    VenuePaymentMethodId = venuePaymentMethodId,
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

        // Get me/order/details
        [HttpGet]
        [Authorize]
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
                            Order = orderTable.Order.Select(o => new
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
                        Order = orderTable.Order.Select(o => new
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
        [Authorize]
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

        // POST me/order
        [HttpPost]
        [Authorize]
        [Route("Me/Order")]
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

                        if (product == null)
                        {
                            return NotFound(new
                            {
                                Success = false,
                                StatusCode = (int)HttpStatusCode.NotFound,
                                Message = "Ürün bulunamadı"
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

                    if (product == null)
                    {
                        return NotFound(new
                        {
                            Success = false,
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Message = "Ürün bulunamadı"
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