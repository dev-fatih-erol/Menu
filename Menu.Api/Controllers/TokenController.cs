using Menu.Core.Models;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    public class TokenController : Controller
    {
        private readonly IWaiterTokenService _waiterTokenService;

        private readonly IUserTokenService _userTokenService;

        public TokenController(IWaiterTokenService waiterTokenService,
            IUserTokenService userTokenService)
        {
            _waiterTokenService = waiterTokenService;

            _userTokenService = userTokenService;
        }

        [HttpGet]
        [Route("Token/User")]
        public IActionResult CreateOrUpdateUserToken(int userId, string token)
        {
            var userToken = _userTokenService.GetByUserId(userId);

            if (userToken != null)
            {
                userToken.Token = token;

                _userTokenService.SaveChanges();

                return Ok(true);
            }

            var newUserToken = new UserToken
            {
                Token = token,
                UserId = userId
            };

            _userTokenService.Create(newUserToken);
            _userTokenService.SaveChanges();

            return Ok(true);
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