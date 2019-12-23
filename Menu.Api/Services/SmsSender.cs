using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Menu.Api.Services
{
    public class SmsSender : ISmsSender
    {
        public async Task<string> Send(string phoneNumber, string message)
        {
            var queryStringParameters = new Dictionary<string, string>()
            {
               {"usercode", "5394257609"},
               {"password", "51A8C2"},
               {"gsmno", phoneNumber },
               {"message", message},
               {"msgheader", "Motto Mobil"},
               {"dil", "TR"}
            };

            var url = QueryHelpers.AddQueryString("https://api.netgsm.com.tr/sms/send/get", queryStringParameters);

            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}