using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        public void RenseignerToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        #region Connexion
        public async Task<LoginResponseModel> SeConnecter(Citoyen citoyen)
        {
            var rangJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new { 
                citoyen.Mail,
                Password = citoyen.MotDePasse
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/login", rangJson);
            httpResponse.EnsureSuccessStatusCode();

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();

            var loginResponse = JObject.Parse(text).ToString();
            return JsonConvert.DeserializeObject<LoginResponseModel>(loginResponse);
        }
        #endregion

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

            using var httpResponse = await Client.PostAsync("/api/rangs", rangJson);
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
                "/api/citoyens/" + id);

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

            using var httpResponse = await Client.PatchAsync("/api/citoyens/" + citoyen.ID, citoyenJson);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteCitoyen(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/citoyens/" + id);
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
                ressource.CategorieID,
            };



            var ressourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(formData),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/api/ressources", ressourceJson);
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

        public async Task UpdateRessource(Ressource ressource)
        {
            var ressourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                ressource.Titre,
                ressource.ValidationAdmin,
                ressource.Contenu,
                ressource.TypeRelationID,
                ressource.CitoyenID,
                ressource.TypeRessourceID,
                ressource.CategorieID

            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PatchAsync("/api/ressources/" + ressource.ID, ressourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task DeleteRessource(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/ressources/" + id);
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

        public async Task UpdateRelation(RelationCitoyen relationCitoyen)
        {
            var relationCitoyenJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                relationCitoyen.CitoyenCibleID,
                relationCitoyen.CitoyenID,
                relationCitoyen.TypeRelationID,
                relationCitoyen.Approbation
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PatchAsync("/api/relations", relationCitoyenJson);
            httpResponse.EnsureSuccessStatusCode();
        }


        public async Task CreateTypeRelation(TypeRelation typeRelation)
        {
            var typeRelationJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(typeRelation),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/api/typeRelations", typeRelationJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteTypeRelation(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/typeRelations/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<RelationCitoyen>> GetRelation(int id)
        {
            var response = await Client.GetAsync(
                 "/api/relations/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var relationJson = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<RelationCitoyen>>(relationJson);
        }

        public async Task<IEnumerable<RelationCitoyen>> GetRelationCitoyenIn(int id)
        {
            var response = await Client.GetAsync(
                 "/api/inrelations/" + id);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var relationJson = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<RelationCitoyen>>(relationJson);
        }
        public async Task<IEnumerable<RelationCitoyen>> GetAllRelations()
        {
            var response = await Client.GetAsync(
                "/api/allrelations");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();
            var relationJson = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<IEnumerable<RelationCitoyen>>(relationJson);
        }
        public async Task AjouterRelation(RelationCitoyen relationCitoyen)
        {
            var typeRelationJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                relationCitoyen.CitoyenID,
                relationCitoyen.CitoyenCibleID,
                relationCitoyen.TypeRelationID,
                relationCitoyen.Approbation
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/api/relations", typeRelationJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteRelation(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/relations/" + id);
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

            using var httpResponse = await Client.PostAsync("/api/typeRessources", typeRessourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteTypeRessource(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/typeRessources/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Commentaires
        #region
        public async Task<IEnumerable<Commentaire>> GetAllCommentaires()
        {
            var response = await Client.GetAsync(
                "/api/commentaires");

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
                "/api/commentaires/" + id);

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

            using var httpResponse = await Client.PostAsync("/api/commentaires", typeRessourceJson);
            httpResponse.EnsureSuccessStatusCode();
        }
        public async Task DeleteCommentaire(int id)
        {
            using var httpResponse = await Client.DeleteAsync("/api/commentaires/" + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion

        //Votes
        #region
        public async Task VoirRessource(int idCitoyen, int idRessource)
        {
            var voirRessourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                CitoyenID = idCitoyen,
                RessourceID = idRessource
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/api/citoyens/views", voirRessourceJson);
        }
        /// <summary>
        /// Ajoute un vote à la ressource cible
        /// </summary>
        /// <param name="idCitoyen"></param>
        /// <param name="idRessource"></param>
        /// <returns></returns>
        public async Task LikerRessource(int idCitoyen, int idRessource)
        {
            var likeRessourceJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                CitoyenID = idCitoyen,
                RessourceID = idRessource
            }),
                Encoding.UTF8,
                "application/json");

            using var httpResponse = await Client.PostAsync("/api/voteRessources", likeRessourceJson);
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

            using var httpResponse = await Client.PostAsync("/api/voteCommentaire", likeCommentaireJson);
        }

        public async Task DeleteLikeCommentaire(int idCitoyen, int idCommentaire)
        {
            using var httpResponse = await Client.DeleteAsync("/api/voteCommentaire/" + idCitoyen + "/" + idCommentaire);
        }
        public async Task DeleteLikeRessource(int idCitoyen, int idRessource)
        {
            using var httpResponse = await Client.DeleteAsync("/api/voteRessources/" + idCitoyen + "/" + idRessource);
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

            using var httpResponse = await Client.PostAsync("/api/tags", rangJson);
            httpResponse.EnsureSuccessStatusCode();

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            StreamReader reader = new(responseStream);
            string text = reader.ReadToEnd();

            var tagCreatedResponse = JObject.Parse(text).SelectToken("data").ToString();
            return JsonConvert.DeserializeObject<Tag>(tagCreatedResponse); ;
        }

        public async Task LierTagEtRessource(int idRessource, int idTag)
        {
            using var httpResponse = await Client.PostAsync("/api/ressources/tags/" + idRessource + "/" + idTag, null);
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
            return JsonConvert.DeserializeObject<Categorie>(categorieResponse);
        }
        #endregion
    }
}
