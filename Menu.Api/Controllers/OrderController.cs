using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public OrderController(ILogger<OrderController> logger,
            IMapper mapper,
            IOrderService orderService)
        {
            _logger = logger;

            _mapper = mapper;

            _orderService = orderService;
        }
    }
}