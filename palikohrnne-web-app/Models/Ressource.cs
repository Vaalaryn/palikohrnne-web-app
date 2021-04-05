using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class Ressource
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string Titre { get; set; }
        public int Vues { get; set; }
        public int Votes { get; set; }
        public string Contenu { get; set; }
        public int TypeRessourceID { get; set; }
        public int TypeRelationID { get; set; }
        public int CitoyenID { get; set; }

        //Relations
        public List<Commentaire> Commentaires { get; set; }
        public List<Citoyen> CitoyenVoted { get; set; }
        public List<Tag> Tags { get; set; }
        public Citoyen Citoyen { get; set; }
    }
}
