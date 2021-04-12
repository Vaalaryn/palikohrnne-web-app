using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class StatistiqueModel
    {
        public IEnumerable<Citoyen> Citoyens { get; set; }
        public IEnumerable<Ressource> Ressources { get; set; }
        public IEnumerable<RelationCitoyen> RelationCitoyens { get; set; }
        public IEnumerable<Commentaire> Commentaires { get; set; }
        public IEnumerable<Citoyen> CitoyenVoteds { get; set; }
    }
}
