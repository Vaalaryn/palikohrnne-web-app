using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class TypeRessource
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Nom { get; set; }
    }
}
