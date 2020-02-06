using System.Linq;
using System.Net;
using Menu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    public class AppController : Controller
    {
        private readonly IAppAboutService _appAboutService;

        private readonly IAppSliderService _appSliderService;

        public AppController(IAppAboutService appAboutService,
            IAppSliderService appSliderService)
        {
            _appAboutService = appAboutService;

            _appSliderService = appSliderService;
        }

        [HttpGet]
        [Authorize]
        [Route("App/Slider")]
        public IActionResult Slider()
        {
            var appSliders = _appSliderService.Get(true);

            if (appSliders.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = appSliders
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Slider bulunamadı"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("App/About")]
        public IActionResult About()
        {
            var appAbouts = _appAboutService.Get(true);

            if (appAbouts.Any())
            {
                return Ok(new
                {
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = appAbouts
                });
            }

            return NotFound(new
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Bilgi bulunamadı"
            });
        }
    }
}