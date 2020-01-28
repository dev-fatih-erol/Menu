using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Menu.Kitchen.Attributes;
using Menu.Kitchen.Extensions;
using Menu.Kitchen.Models.HomeViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Kitchen.Controllers
{
    public class HomeController : Controller
    {
        private readonly IKitchenService _kitchenService;

        public HomeController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        [HttpGet]
        [Route("")]
        [AnonymousOnly]
        [AllowAnonymous]
        [ImportModelState]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [AnonymousOnly]
        [AllowAnonymous]
        [ExportModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var kitchen = _kitchenService.GetByUsernameAndPassword(model.Username, model.Password.ToMD5());

            if (kitchen != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, kitchen.Id.ToString())
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(identity),
                          new AuthenticationProperties
                          {
                              ExpiresUtc = DateTime.Now.AddDays(1),
                              IsPersistent = true
                          });

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Girdiğin kullanıcı adı veya şifre hiçbir hesapla eşleşmiyor. Hesap bilgilerinizi unuttuysanız lütfen bizimle iletişime geçin.");

            return RedirectToAction("Index", "Home");
        }
    }
}