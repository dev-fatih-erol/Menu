using System;
using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.TableViewModels;
using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        [Authorize]
        [Route("Table")]
        public IActionResult Index()
        {

            var model = _tableService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
            {
                Name = x.Name,
                CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                Id = x.Id,
            });

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("Table/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var table = _tableService.GetById(id);

            if (table != null)
            {
                if (table.VenueId == User.Identity.GetVenueId())
                {


                    var model = new EditViewModel()
                    {
                        Name = table.Name,


                    };

                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("Table/Edit/{id:int}")]
        public IActionResult Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var table = _tableService.GetById(id);

                if (table != null)
                {
                    if (table.VenueId == User.Identity.GetVenueId())
                    {
                        table.Name = model.Name;

                        _tableService.SaveChanges();


                        return RedirectToAction("Index");
                    }
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Table/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("Table/Create")]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var table = new Table
                {
                    TableStatus = Core.Enums.TableStatus.Closed,
                    Name = model.Name,
                    VenueId = User.Identity.GetVenueId(),
                    CreatedDate = DateTime.Now
                };

                _tableService.Create(table);

                _tableService.SaveChanges();

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

    }
}