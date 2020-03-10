﻿using System;
using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.WaiterViewModels;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class WaiterController : Controller
    {
        private readonly IWaiterService _waiterService;

        private readonly ITableWaiterService _tableWaiterService;

        private readonly ITableService _tableService;

        private readonly IWaiterTokenService _waiterTokenService;

        public WaiterController(IWaiterService waiterService,
           ITableWaiterService tableWaiterService,
           ITableService tableService,
           IWaiterTokenService waiterTokenService)
        {
            _waiterService = waiterService;

            _tableWaiterService = tableWaiterService;

            _tableService = tableService;

            _waiterTokenService = waiterTokenService;
        }

        [HttpGet]
        [Authorize]
        [Route("Waiter/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var waiter = _waiterService.GetById(id);

            if (waiter != null)
            {
                var tables = _tableWaiterService.GetByWaiterId(id);

                if (waiter.VenueId == User.Identity.GetVenueId())
                {
                    var model = new EditViewModel()
                    {
                        Name = waiter.Name,
                        Surname = waiter.Surname,
                        Username = waiter.Username,
                        Password = waiter.Password,
                        TableViewModels = tables.Select(x => new TableViewModel
                        {
                            Id = x.Table.Id,
                            Name = x.Table.Name,
                            Selected = true
                        }).ToArray()
                    };

                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize]
        [Route("Waiter/Create")]
        public IActionResult Create()
        {
            var tables = _tableService.GetByVenueId(User.Identity.GetVenueId());

            var model = new CreateViewModel()
            {
                TableViewModels = tables.Select(x => new TableViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Selected = false
                }).ToArray()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("Waiter/Create")]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newWaiter = new Waiter
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Username = model.Username,
                    Password = model.Password.ToMD5(),
                    VenueId = User.Identity.GetVenueId(),
                    CreatedDate = DateTime.Now
                };

                _waiterService.Create(newWaiter);

                _waiterService.SaveChanges();

                foreach (var table in model.TableViewModels)
                {
                    if (table.Selected)
                    {
                        var newTableWaiter = new TableWaiter
                        {
                            TableId = table.Id,
                            WaiterId = newWaiter.Id,
                            CreatedDate = DateTime.Now
                        };

                        _tableWaiterService.Create(newTableWaiter);

                        _tableWaiterService.SaveChanges();
                    }
                }

                _waiterTokenService.Create(new WaiterToken
                {
                    Token = "0",
                    WaiterId = newWaiter.Id
                });

                _waiterTokenService.SaveChanges();

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Waiter")]
        public IActionResult Index()
        {
            var model = _waiterService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Username = x.Username,
                CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                Tables = _tableWaiterService.GetByWaiterId(x.Id).Select(x => x.Table.Name)
            });

            return View(model);
        }
    }
}