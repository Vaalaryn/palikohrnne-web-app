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
            //client.BaseAddress = new Uri("http://palikorne.brice-bitot.fr/");
            client.BaseAddress = new Uri("http://localhost:8081/");
            Client = client;
        }
        //Rangs
        #region
        public async Task<IEnumerable<Rang>> GetAllRangs()
        {
            var response = await Client.GetAsync(
                "/rangs");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
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

        public async Task CreateRang(Rang rang)
        {
            var rangJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { rang.Nom }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/rangs", rangJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Citoyens
        #region
        public async Task<IEnumerable<Citoyen>> GetAllCitoyens()
        {
            var response = await Client.GetAsync(
                "/citoyens");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
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
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteCitoyen(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/citoyens/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Ressources
        #region
        public async Task<IEnumerable<Ressource>> GetAllRessources()
        {
            var response = await Client.GetAsync(
                "/ressources");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var ressources = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Ressource>>(ressources);
        }
        public async Task<Ressource> GetRessourceById(int id)
        {
            var response = await Client.GetAsync(
                "/ressources/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var ressource = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Ressource>(ressource);
        }

        public async Task CreateRessource(Ressource ressource)
        {
            var ressourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { 
                ressource.Titre,
                ressource.Contenu,
                ressource.CitoyenID,
                ressource.TypeRelationID,
                ressource.TypeRessourceID
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/ressources", ressourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteRessource(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/ressources/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Relations
        #region
        public async Task<IEnumerable<TypeRelation>> GetAllTypeRelations()
        {
            var response = await Client.GetAsync(
                "/typeRelations");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var typeRelations = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<TypeRelation>>(typeRelations);
        }
        public async Task<TypeRelation> GetTypeRelationById(int id)
        {
            var response = await Client.GetAsync(
                "/typeRelations/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var typeRelation = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<TypeRelation>(typeRelation);
        }

        public async Task CreateTypeRelation(TypeRelation typeRelation)
        {
            var typeRelationJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(typeRelation),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/typeRelations", typeRelationJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteTypeRelation(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/typeRelations/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //TypeRessource
        #region
        public async Task<IEnumerable<TypeRessource>> GetAllTypeRessources()
        {
            var response = await Client.GetAsync(
                "/typeRessources");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var typeRessources = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<TypeRessource>>(typeRessources);
        }

        public async Task<TypeRessource> GetTypeRessourceById(int id)
        {
            var response = await Client.GetAsync(
                "/typeRessources/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var typeRessource = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<TypeRessource>(typeRessource);
        }

        public async Task CreateTypeRessource(TypeRessource typeRessource)
        {
            var typeRessourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                typeRessource.Nom,
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/typeRessources", typeRessourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteTypeRessource(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/typeRessources/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion
    }
}
