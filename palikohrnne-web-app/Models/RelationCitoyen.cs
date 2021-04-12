using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class RelationCitoyen
    {
        public int CitoyenID { get; set; }
        public Citoyen Citoyen { get; set; }
        public int CitoyenCibleID { get; set; }
        public Citoyen CitoyenCible { get; set; }
        public int TypeRelationID { get; set; }
        public TypeRelation TypeRelation { get; set; }
        public bool? Approbation { get; set; }
    }
}
