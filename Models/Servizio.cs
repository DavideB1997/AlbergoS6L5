using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbergoS6L5.Models
{
    public class Servizio
    {
        public int IdServizio { get; set; }
        public string Descrizione { get; set; }
        public int Costo { get; set; }

        public int Quantita { get; set; }
    }

    public class Camera
    {
        public int IdCamera { get; set; }
        public string TipologiaCamera { get; set; }
        public string Descrizione { get; set; }
        public int Prezzo { get; set; }
    }

}