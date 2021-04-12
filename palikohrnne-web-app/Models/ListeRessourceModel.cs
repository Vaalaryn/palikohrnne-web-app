using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class ListeRessourceModel
    {
        public List<Ressource> Ressources { get; set; }
        public FiltresModelRessources Filtres { get; set;}
    }
}
