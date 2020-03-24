using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class WinkelmandItem
    {
        public int KlantNr { get; set; }

        public string naam { get; set; }

        public int ArtikelNr { get; set; }

        public int? Aantal { get; set; }

        public string Foto { get; set; }

        public double Prijs { get; set; }

        public double totaal { get; set; }
    }
}
