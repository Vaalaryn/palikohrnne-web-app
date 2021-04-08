using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class Citoyen
    {
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
        public string Ville { get; set; }
        public int RangID { get; set; }
        public Rang Rang { get; set; }
        //public Rang Rang { get; set; }
    }
}
