using Menu.Kitchen.Extensions;
using Menu.Kitchen.Models.ComponentViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Kitchen.Components
{
    public class Profile : ViewComponent
    {
        private readonly IKitchenService _kitchenService;

        public Profile(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        public IViewComponentResult Invoke()
        {
            var kitchen = _kitchenService.GetById(User.Identity.GetId());

            var model = new ProfileViewModel
            {
                KitchenName = kitchen.Name,
                VenueName = kitchen.Venue.Name,
                VenuePhoto = kitchen.Venue.Photo,
                VenueId = kitchen.Venue.Id
            };

            return View(model);
        }
    }
}