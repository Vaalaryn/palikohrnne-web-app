using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Api
{
    public class CubesService
    {
        public HttpClient Client { get; }

        public CubesService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://brice-bitot.fr");
            //client.BaseAddress = new Uri("http://localhost:8080/");
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
        public async Task<Rang> GetRangById(int id)
        {
            var response = await Client.GetAsync(
                "/rangs/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var rang = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Rang>(rang);
        }

        //Citoyens
        #region
        public async Task<IEnumerable<Citoyen>> GetAllCitoyens()
        {
            var response = await Client.GetAsync(
                "/citoyens");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();
            var citoyens = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Citoyen>>(citoyens);
        }

        public async Task<Citoyen> GetCitoyenById(int id)
        {
            var response = await Client.GetAsync(
                "/citoyens/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var citoyens = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Citoyen>(citoyens);
        }

        public async Task CreateCitoyen(Citoyen citoyen)
        {
            var citoyenJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(citoyen),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/citoyens", citoyenJson);
            Console.WriteLine(httpResponse.Content.ReadAsStringAsync().Result);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task UpdateCitoyen(Citoyen citoyen)
        {
            var citoyenJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                citoyen.Adresse,
                citoyen.CodePostal,
                citoyen.Genre,
                citoyen.Mail,
                citoyen.MotDePasse,
                citoyen.Nom,
                citoyen.Prenom,
                citoyen.Pseudo,
                citoyen.Telephone,
                citoyen.Ville,
                citoyen.RangID
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PatchAsync("/citoyens/" + citoyen.ID, citoyenJson);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteCitoyen(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/citoyens/2");
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        public async Task<IEnumerable<Ressource>> GetAllRessources()
        {
            var response = await Client.GetAsync(
                "/ressources");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();
            var ressources = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Ressource>>(ressources);
        }

        public async Task CreateRessource(Ressource ressource)
        {
            var ressourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(ressource),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/ressources", ressourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }


        public async Task CreateTypeRelation(TypeRelation typeRelation)
        {
            var typeRelationJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(typeRelation),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/typeRelations", typeRelationJson);
            httpResponse.EnsureSuccessStatusCode();
        }

    }
}
