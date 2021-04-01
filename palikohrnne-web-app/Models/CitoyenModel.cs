using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class CitoyenModel
    {
        public CitoyenModel(JToken item)
        {
            ID = item["ID"].ToObject<int>();
            CreatedAt = item["CreatedAt"].ToObject<DateTime>();
            UpdatedAt = item["UpdatedAt"].ToObject<DateTime>();
            DeletedAt = item["DeletedAt"].ToObject<DateTime?>();
            Adresse = item["Adresse"].ToObject<string>();
            CodePostal = item["CodePostal"].ToObject<string>();
            Genre = item["Genre"].ToObject<string>();
            Mail = item["Mail"].ToObject<string>();
            MotDePasse = item["MotDePasse"].ToObject<string>();
            Nom = item["Nom"].ToObject<string>();
            Prenom = item["Prenom"].ToObject<string>();
            Pseudo = item["Pseudo"].ToObject<string>();
            Telephone = item["Telephone"].ToObject<string>();
            RangId = item["RangID"].ToObject<int>();
        }

        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Adresse { get; set; }
        public string CodePostal { get; set; }
        public string Genre { get; set; }
        public string Mail { get; set; }
        public string MotDePasse { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }        
        public string Pseudo { get; set; }        
        public string Telephone { get; set; }        
        public int RangId { get; set; }

        
    }
}
