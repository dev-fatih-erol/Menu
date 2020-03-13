using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.KitchenViewModels;
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

        [HttpPost]
        [Authorize]
        [Route("Kitchen/Edit/{id:int}")]
        public IActionResult Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kitchen = _kitchenService.GetByIds(id);

                if (kitchen != null)
                {
                    if (kitchen.VenueId == User.Identity.GetVenueId())
                    {
                        kitchen.Name = model.Name;
                        kitchen.Password = model.Password.ToMD5();
                        kitchen.Username = model.Username;

                        _kitchenService.SaveChanges();


                        return RedirectToAction("Index");
                    }
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Kitchen")]
        public IActionResult Index()
        {

            var model = _kitchenService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
            {
                Name = x.Name,
                Username = x.Username,
                CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                Id = x.Id,
            });

            return View(model);
        }


        [HttpGet]
        [Authorize]
        [Route("Kitchen/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var kitchen = _kitchenService.GetByIds(id);

            if (kitchen != null)
            {
                if (kitchen.VenueId == User.Identity.GetVenueId())
                {


                    var model = new EditViewModel()
                    {
                        Name = kitchen.Name,
                        Username = kitchen.Username,
                        Password = kitchen.Password,

                    };

                    return View(model);
                }
            }

            return NotFound();
        }
    }
}