using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class CategorieWithStats
    {
        public Categorie Categorie { get; set; }
        public Ressource LastRessource { get; set; }
        public int TotalCommentaires { get; set; }
        public int TotalRessources { get; set; }
    }
}
