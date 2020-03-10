using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class KitchenController : Controller
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }
    }
}