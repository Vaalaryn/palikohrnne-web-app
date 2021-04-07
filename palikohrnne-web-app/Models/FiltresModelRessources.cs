using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class FiltresModelRessources
    {
        public string AnswersFilter { get; set; }
        public string AgeMax { get; set; }
        public List<int> TypeRelationID { get; set; }
        public List<int> TagsID { get; set; }
    }
}
