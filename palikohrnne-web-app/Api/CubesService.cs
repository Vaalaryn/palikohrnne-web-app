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

        public async Task<Ressource> CreateRessource(Ressource ressource)
        {
            var formData = new
            {
                ressource.Titre,
                ressource.Contenu,
                ressource.CitoyenID,
                ressource.TypeRelationID,
                ressource.TypeRessourceID, 
                ressource.CategorieID
            };

            

            var ressourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(formData),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/ressources", ressourceJson);
            httpResponse.EnsureSuccessStatusCode();

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();

            var ressourceCreatedResponse = JObject.Parse(text).SelectToken("data").ToString();
            Ressource ressourceCreated = JsonConvert.DeserializeObject<Ressource>(ressourceCreatedResponse);

            foreach (var tag in ressource.Tags)
            {
                await LierTagEtRessource(ressourceCreated.ID, tag.ID);
            }

            return ressourceCreated;
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

        //Commentaires
        #region
        public async Task<IEnumerable<Commentaire>> GetAllCommentaires()
        {
            var response = await Client.GetAsync(
                "/commentaires");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var commentaires = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Commentaire>>(commentaires);
        }

        public async Task<Commentaire> GetCommentaireById(int id)
        {
            var response = await Client.GetAsync(
                "/commentaires/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var commentaire = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Commentaire>(commentaire);
        }

        public async Task CreateCommentaire(Commentaire commentaire)
        {
            var typeRessourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                commentaire.RessourceID,
                commentaire.CitoyenID,
                commentaire.Contenu
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/commentaires", typeRessourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteCommentaire(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/commentaires/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Votes
        #region
        /// <summary>
        /// Ajoute un vote à la ressource cible
        /// </summary>
        /// <param name="idCitoyen"></param>
        /// <param name="idRessource"></param>
        /// <returns></returns>
        public async Task LikerRessource(int idCitoyen, int idRessource)
        {
            var likeRessourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new {
                CitoyenID = idCitoyen,
                RessourceID = idRessource
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/voteRessources", likeRessourceJson);
        }
        /// <summary>
        /// Ajoute un vote au commentaire cible
        /// </summary>
        /// <param name="idCitoyen"></param>
        /// <param name="idCommentaire"></param>
        /// <returns></returns>
        public async Task LikerCommentaire(int idCitoyen, int idCommentaire)
        {
            var likeCommentaireJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                CitoyenID = idCitoyen,
                CommentaireID = idCommentaire
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/voteCommentaire", likeCommentaireJson);
        }

        public async Task DeleteLikeCommentaire(int idCitoyen, int idCommentaire)
        {
            using var httpResponse = await Client.DeleteAsync("/voteCommentaire/" + idCitoyen + "/" + idCommentaire);
        }
        public async Task DeleteLikeRessource(int idCitoyen, int idRessource)
        {
            using var httpResponse = await Client.DeleteAsync("/voteRessources/" + idCitoyen + "/" + idRessource);
        }
        #endregion

        //Tags
        #region
        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            var response = await Client.GetAsync(
                "/tags");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var tags = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<Tag>>(tags);
        }

        public async Task<Tag> GetTagById(int id)
        {
            var response = await Client.GetAsync(
                "/tags/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var tag = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Tag>(tag);
        }

        public async Task<Tag> CreateTag(Tag tag)
        {
            var rangJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { tag.Nom }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/tags", rangJson);
            httpResponse.EnsureSuccessStatusCode();

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();

            var tagCreatedResponse = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Tag>(tagCreatedResponse); ;
        }

        public async Task LierTagEtRessource(int idRessource,int idTag)
        {
            using var httpResponse = await Client.PostAsync("/ressources/tags/" + idRessource + "/" + idTag, null);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Categories
        #region
        public async Task<IEnumerable<CategorieWithStats>> GetAllCategoriesWithStats()
        {
            var response = await Client.GetAsync(
            "/categories");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var categories = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<CategorieWithStats>>(categories);
        }
        public async Task<Tag> GetCategorieWithStatsById(int id)
        {
            var response = await Client.GetAsync(
                "/categories/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var categories = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Tag>(categories);
        }
        public async Task<Categorie> CreateCategorie(Categorie categorie)
        {
            var categorieJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { categorie.Nom, categorie.Description }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/categories", categorieJson);
            httpResponse.EnsureSuccessStatusCode();

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();

            var categorieResponse = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Categorie>(categorieResponse); ;
        }
        #endregion
    }
}
