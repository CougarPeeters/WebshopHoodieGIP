using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class VMToevoegen
    {
        
        public int? Aantal { get; set; }
        
        public Artikel GeselecteerdArtikel { get; set; }
       
    }
}
