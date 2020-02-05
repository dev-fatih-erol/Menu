using Menu.Cash.Extensions;
using Menu.Cash.Models.ComponentViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Cash.Components
{
    public class Profile : ViewComponent
    {
        private readonly ICashService _cashService;

        public Profile(ICashService cashService)
        {
            _cashService = cashService;
        }

        public IViewComponentResult Invoke()
        {
            var cash = _cashService.GetById(User.Identity.GetId());

            var model = new ProfileViewModel
            {
                CashName = cash.Name,
                VenueName = cash.Venue.Name,
                VenuePhoto = cash.Venue.Photo,
                VenueId = cash.Venue.Id
            };

            return View(model);
        }
    }
}