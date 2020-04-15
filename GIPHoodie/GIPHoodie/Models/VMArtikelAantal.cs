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

        //public string foutmelding()
        //{
        //    int voorraad = GeselecteerdArtikel.Voorraad;
            
        //    if (Aantal <= 0)
        //    {
        //        return "Moet een positief getal zijn";
        //    }
        //    else if(Aantal > voorraad)
        //    {
        //        return "Niet genoeg op voorraad";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }
}
