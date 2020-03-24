using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class Klant
    {
        public int KlantID { get; set; }

        public string Naam { get; set; }

        public string Adres { get; set; }

        public int PostCode { get; set; }

        public string Gemeente { get; set; }

        public string Mail { get; set; }

        public string Gebruikersnaam { get; set; }

        public string Wachtwoord { get; set; }
    }
}
