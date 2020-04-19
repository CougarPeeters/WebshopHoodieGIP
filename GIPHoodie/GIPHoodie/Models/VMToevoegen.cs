using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class VMToevoegen
    {
        
        [Required(ErrorMessage ="Verplicht veld.")]
        public int? Aantal { get; set; }
        public string invoer { get; set; }
        public Artikel GeselecteerdArtikel { get; set; }
       
    }
}
