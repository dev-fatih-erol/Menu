using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Menu.Cash.Attributes;
using Menu.Cash.Extensions;
using Menu.Cash.Models.HomeViewModels;
using Menu.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Cash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICashService _cashService;

        public HomeController(ICashService cashService)
        {
            _cashService = cashService;
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

            var cash = _cashService.GetByUsernameAndPassword(model.Username, model.Password.ToMD5());

            if (cash != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, cash.Id.ToString())
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

        [HttpPost]
        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}