using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GIPHoodie.Models
{
    public class VMArtikelAantal
    {
        [Required (ErrorMessage = "Verplicht veld.")]
        public int? Aantal { get; set; }

        public Artikel GeselecteerdArtikel { get; set; }
    }
}
