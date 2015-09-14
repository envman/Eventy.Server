using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EventServer.Api.Controllers;
using EventServer.Web.Controllers;
using Newtonsoft.Json;

namespace EventServer.Web.Services
{
    public class EventService
    {
        private string _url = "http://joinin.azurewebsites.net";
        //private string _url = "http://localhost:1436";

        private readonly JsonWebToken _token;

        public EventService(JsonWebToken token)
        {
            _token = token;
        }

        public IEnumerable<EventHeader> ListEvents()
        {
            using (var client = GetClient())
            {
                var response = client.GetAsync($"{_url}/api/Event").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<EventHeader>>(content);
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);
            return client;
        }
    }
}
