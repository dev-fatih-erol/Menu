using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Menu.Business.Extensions;
using Menu.Business.Models.HomeViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Business.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManagerService _managerService;

        private readonly IVenueService _venueService;

        public HomeController(IManagerService managerService,
            IVenueService venueService)
        {
            _managerService = managerService;

            _venueService = venueService;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var manager = _managerService.GetByUsernameAndPassword(model.Username, model.Password.ToMD5());

            if (manager != null)
            {
                var venueId = _venueService.GetByManagerId(manager.Id).Id;
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, manager.Id.ToString()),
                        new Claim("VenueId", venueId.ToString())
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(identity),
                          new AuthenticationProperties
                          {
                              ExpiresUtc = DateTime.UtcNow.AddYears(1),
                              IsPersistent = true
                          });

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Girdiğin kullanıcı adı veya şifre hiçbir hesapla eşleşmiyor. Hesap bilgilerinizi unuttuysanız lütfen bizimle iletişime geçin.");

            return View(model);
        }
    }
}