using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Menu.Api.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Menu.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IMapper _mapper;

        private readonly IOrderService _orderService;

        private readonly IOrderDetailService _orderDetailService;

        private readonly IVenueService _venueService;

        private readonly ITableService _tableService;

        private readonly IWaiterService _waiterService;

        private readonly IProductService _productService;

        private readonly IOptionItemService _optionItemService;

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
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
            if (dto.OrderDetail.Any())
            {
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Ürün bulunamadı"
            });
        }
    }
}