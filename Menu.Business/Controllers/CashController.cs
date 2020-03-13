using System.Linq;
using Menu.Business.Extensions;
using Menu.Business.Models.CashViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class CashController : Controller
    {
        private readonly ICashService _CashService;

        public CashController(ICashService cashService)
        {
            _CashService = cashService;
        }


        [HttpGet]
        [Authorize]
        [Route("Cash")]
        public IActionResult Index()
        {

            var model = _CashService.GetByVenueId(User.Identity.GetVenueId()).Select(x => new IndexViewModel
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
        [Route("Cash/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var cash = _CashService.GetByIds(id);

            if (cash != null)
            {
                if (cash.VenueId == User.Identity.GetVenueId())
                {


                    var model = new EditViewModel()
                    {
                        Name = cash.Name,
                        Username = cash.Username,
                        Password = cash.Password,

                    };

                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("Cash/Edit/{id:int}")]
        public IActionResult Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cash = _CashService.GetByIds(id);

                if (cash != null)
                {
                    if (cash.VenueId == User.Identity.GetVenueId())
                    {
                        cash.Name = model.Name;
                        cash.Password = model.Password.ToMD5();
                        cash.Username = model.Username;

                        _CashService.SaveChanges();


                        return RedirectToAction("Index");
                    }
                }

                return NotFound();
            }

            return BadRequest();
        }
    }
}