using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class Artikel
    {
        public int ArtikelID { get; set; }

        public string Naam { get; set; }

        public int Voorraad { get; set; }

        public double Prijs { get; set; }

        public string Foto { get; set; }
    }
}
