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

        private readonly IProductService _productService;

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderService orderService,
            IProductService productService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderService = orderService;

            _productService = productService;
        }

        // POST order
        [HttpPost]
        [Route("Order")]
        public IActionResult Create()
        {
            int[] ids = new int[] { 9119, 9811, 92111, 97111, 95111 };

            var products = _productService.GetByIds(ids);

            if (products.Any())
            {
                var newOrder = new Order
                {
                    Code = "code1",
                    Description = "Descc",
                    CreatedDate = DateTime.Now,
                    UserId = User.Identity.GetId(),
                    WaiterId = 1,
                    TableId = 1,
                    VenueId = 1,
                    OrderStatusId = 1
                };

                _orderService.Create(newOrder);

                _orderService.SaveChanges();

                foreach (var product in products)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        Name = product.Name,
                        Photo = product.Photo,
                        Quantity = 1,
                        Price = product.Price
                    };
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