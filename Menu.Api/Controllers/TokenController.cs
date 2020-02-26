using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    public class TokenController : Controller
    {
        private readonly IWaiterTokenService _waiterTokenService;

        public TokenController(IWaiterTokenService waiterTokenService)
        {
            _waiterTokenService = waiterTokenService;
        }

        [HttpGet]
        [Route("Token/Waiter")]
        public IActionResult CreateOrUpdateWaiterToken(int waiterId, string token)
        {
            var waiterToken = _waiterTokenService.GetByWaiterId(waiterId);

            if (waiterToken != null)
            {
                waiterToken.Token = token;

                _waiterTokenService.SaveChanges();

                return Ok(true);
            }

            var newWaiterToken = new WaiterToken
            {
                Token = token,
                WaiterId = waiterId
            };

            _waiterTokenService.Create(newWaiterToken);
            _waiterTokenService.SaveChanges();

            return Ok(true);
        }
    }
}