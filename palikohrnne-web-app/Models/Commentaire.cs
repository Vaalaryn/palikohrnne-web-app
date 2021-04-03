using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class Commentaire
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int CitoyenID { get; set; }
        public int? ParentID { get; set; }
        public int RessourceID { get; set; }
        public string Contenu { get; set; }
        public int Votes { get; set; }
    }
}
