using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbergoS6L5.Models
{
    public class ServizioAggiuntivo
    {
        public int IdServizioRichiesti { get; set; }
        public int IdPrenotazione { get; set; }
        public int Quantita { get; set; }
        public int IdServizio {  get; set; }

    }
}