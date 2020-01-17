using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Extensions;
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

        private readonly IVenueService _venueService;

        private readonly ITableService _tableService;

        private readonly IWaiterService _waiterService;

        private readonly IProductService _productService;

        private readonly IOptionItemService _optionItemService;

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderTableService orderTableService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IVenueService venueService,
            ITableService tableService,
            IWaiterService waiterService,
            IProductService productService,
            IOptionItemService optionItemService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderTableService = orderTableService;

            _orderService = orderService;

            _orderDetailService = orderDetailService;

            _venueService = venueService;

            _tableService = tableService;

            _waiterService = waiterService;

            _productService = productService;

            _optionItemService = optionItemService;
        }

        // POST order
        [HttpPost]
        [Route("Order")]
        public IActionResult Create([FromBody] CreateOrderDto dto)
        {
            var orderTable = _orderTableService.GetByTableIdAndVenueId(dto.TableId, dto.VenueId, false);

            if (orderTable == null)
            {
                var newOrderTable = new OrderTable
                {
                    IsClosed = false,
                    TableId = dto.TableId,
                    VenueId = dto.VenueId
                };

                _orderTableService.Create(newOrderTable);

                _orderTableService.SaveChanges();
            }

            orderTable = _orderTableService.GetByTableIdAndVenueId(dto.TableId, dto.VenueId, false);

            var newOrder = new Order
            {
                Code = Guid.NewGuid().ToString(),
                Description = dto.Description,
                OrderStatus = OrderStatus.Pending,
                CreatedDate = DateTime.Now,
                UserId = 1,
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
                        Order = newOrder
                    };

                    _orderDetailService.Create(newOrderDetail);
                }
            }

            _orderService.Create(newOrder);

            _orderService.SaveChanges();

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }
    }
}