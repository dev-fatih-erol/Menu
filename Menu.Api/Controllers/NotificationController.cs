using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Menu.Service;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Api.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ITableWaiterService _tableWaiterService;

        private readonly string _key = "key=AAAA7Tr-w-A:APA91bFkdAPrjKgsrKdzqFpR1EXzmie3oUk6KaVgaPmdCyNdOsik_zyMJZHo2MgAAXYShzwJjj1dnlPpn-DvhW5JnYyzwDyahdVV9FyoHYV4K6XUggKJTm0uXRLxVhodorwKEzThBkqc";

        public NotificationController(ITableWaiterService tableWaiterService)
        {
            _tableWaiterService = tableWaiterService;
        }

        [HttpGet]
        [Route("Notification/CallWaiter")]
        public async Task<IActionResult> CallWaiter(int tableId)
        {
            var waiters = _tableWaiterService.GetByTableId(tableId);

            var tokens = waiters.Select(s => s.Waiter.WaiterToken.Token).ToList();

            if (tokens.Count() > 0 || tokens != null)
            {
                dynamic foo = new ExpandoObject();
                foo.registration_ids = tokens;
                foo.notification = new
                {
                    title = "Çağrı var",
                    body = waiters.Select(x => x.Table.Name).FirstOrDefault()  + " isimli masadan çağrı var",
                    data = new { tableId }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _key);

                var response = await httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", stringContent);

                await response.Content.ReadAsStringAsync();

                return Ok(true);
            }

            return Ok(false);
        }
    }
}