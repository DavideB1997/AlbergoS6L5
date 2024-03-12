using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbergoS6L5.Models
{
    public class Prenotazione
    {
        public int IdPrenotazione { get; set; }
        public int IdCliente { get; set; }

        public DateTime DataPrenotazione { get; set; }
        public int Anno {  get; set; }

        public DateTime SoggiornoInizio { get; set; }
        public DateTime SoggiornoFine { get; set; }
        public int Caparra {  get; set; }
        public string Tariffa { get; set; }
        public int IdServizi { get; set; }
        public string Dettagli { get; set; }
        public int IdCamera { get; set; }

        public int Saldo { get; set;}

        public bool Pagato { get; set; }
    }


    public class PrenotazioneOption
    {
        public int IdPrenotazione { get; set; }
        public int IdCliente { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }
    }

    public class PrenotazioneCheckOut
    {
        public int Stanza { get; set; }
        public DateTime SoggiornoInizio { get; set; }
        public DateTime SoggiornoFine { get; set; }

        public string Tariffa { get; set; }

    }


}