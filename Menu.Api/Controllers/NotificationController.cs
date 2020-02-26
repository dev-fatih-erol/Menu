using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    public class NotificationController : Controller
    {
        // GET: api/values
        [HttpPost]
        [Route("Notification")]
        public async Task<IActionResult> Send(string to)
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://fcm.googleapis.com/fcm/send");

            await response.Content.ReadAsStringAsync();

            return Ok("");
        }
    }
}