using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class RelationsModel
    {
        public List<RelationCitoyen> Relations { get; set; }
        public List<RelationCitoyen> InRelation { get; set; }
    }
}
