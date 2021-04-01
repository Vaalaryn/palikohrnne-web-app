using Newtonsoft.Json.Linq;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Api
{
    public static class ApiCube
    {
        public static string baseUrl = "http://palikorne.brice-bitot.fr/";


        public static async Task<JToken> CallApi(string route, int? id = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl + route + (id.HasValue ? "/" + id : "")))
                    {
                        //Then get the data or content from the response in the next using statement, then within it you will get the data, and convert it to a c# object.
                        using (HttpContent content = res.Content)
                        {
                            //Now assign your content to your data variable, by converting into a string using the await keyword.
                            var data = await content.ReadAsStringAsync();
                            //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
                            if (data != null)
                            {
                                return JObject.Parse(data)["data"];
                            }
                            else
                            {
                                return JToken.Parse("{}");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return JToken.Parse("{error: " + exception.Message + "}");
            }
        }

        //CITOYEN --------------
        #region
        public static async Task<List<CitoyenModel>> GetCitoyens()
        {
            List<CitoyenModel> citoyens = new List<CitoyenModel>();

            var result = await CallApi("citoyens");

            JArray resultStr = JArray.Parse(result.ToString());
            foreach (var item in result)
            {
                citoyens.Add(new CitoyenModel(item));
            }
            return citoyens;
        }

        public static async Task<CitoyenModel> GetCitoyen(int id)
        {
            return new CitoyenModel(await CallApi("citoyens", id));
        }
        #endregion
        //-------------------
    }
}