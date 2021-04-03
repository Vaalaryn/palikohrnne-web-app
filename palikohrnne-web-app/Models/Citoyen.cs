using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Adresse { get; set; }
        [Required]
        [Display(Name = "Code Postal")]
        public string CP { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string MotDePasse { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Pseudo { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Ville { get; set; }
        public int RangID { get; set; }
        //public Rang Rang { get; set; }
    }
}
