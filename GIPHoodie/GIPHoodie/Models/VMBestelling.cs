using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class VMBestelling
    {
        public Bestelling Bestelling { get; set; }
        public Klant klant { get; set; }
    }
}
