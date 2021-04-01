using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Api
{
    public class CubesService
    {
        public HttpClient Client { get; }

        public CubesService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://palikorne.brice-bitot.fr/");
            Client = client;
        }

        public async Task<IEnumerable<Rang>> GetAllRangs()
        {
            var response = await Client.GetAsync(
                "/rangs");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();
            var rangs = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Rang>>(rangs);
        }
    }
}
