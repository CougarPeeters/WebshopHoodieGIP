using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class Bestelling
    {
        public int OrderID { get; set; }
        public DateTime Orderdatum { get; set; }
        public int KlantID { get; set; }

    }
}
