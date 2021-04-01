using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class Rang
    {
        public Rang(JToken item)
        {
            ID = item["ID"].ToObject<int>();
            CreatedAt = item["CreatedAt"].ToObject<DateTime>();
            UpdatedAt = item["UpdatedAt"].ToObject<DateTime>();
            DeletedAt = item["DeletedAt"].ToObject<DateTime?>();
            Nom = item["Nom"].ToString();
        }

        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Nom { get; set; }

    }
}