using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Menu.Api.Extensions;
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

        private readonly IOrderService _orderService;

        private readonly IVenueService _venueService;

        private readonly IProductService _productService;

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderService orderService,
            IVenueService venueService,
            IProductService productService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderService = orderService;

            _venueService = venueService;

            _productService = productService;
        }

        // POST order
        [HttpPost]
        [Route("Order")]
        public IActionResult Create()
        {
            var venue = _venueService.GetById(1);

            if (venue != null)
            {
                var product = _productService.GetById(1, 1);

                if (product != null)
                {

                }
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