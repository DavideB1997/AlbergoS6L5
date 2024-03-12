using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlbergoS6L5.Models;

namespace AlbergoS6L5.Controllers
{
    public class PrenotazioneController : Controller
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();














        // GET: Prenotazione
        public ActionResult AddPrenotazione(Prenotazione prenotazione, Cliente cliente)
        {

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Comando per inserire il cliente
                var command = new SqlCommand("INSERT INTO Cliente (CF,Cognome,Nome,Citta,Provincia,Email,Telefono,Cellulare) VALUES (@cF, @cognome, @nome, @citta, @provincia, @email, @telefono, @cellulare) ; SELECT SCOPE_IDENTITY();", conn);

                command.Parameters.Add("@cF", SqlDbType.NVarChar).Value = cliente.CF;
                command.Parameters.Add("@cognome", SqlDbType.NVarChar).Value = cliente.Cognome;
                command.Parameters.Add("@nome", SqlDbType.NVarChar).Value = cliente.Nome;
                command.Parameters.Add("@citta", SqlDbType.NVarChar).Value = cliente.Citta;
                command.Parameters.Add("@provincia", SqlDbType.NVarChar).Value = cliente.Provincia;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = cliente.Email;
                command.Parameters.Add("@telefono", SqlDbType.NVarChar).Value = (object)cliente.Telefono ?? DBNull.Value;
                command.Parameters.Add("@cellulare", SqlDbType.NVarChar).Value = cliente.Cellulare;



                //// Ottiene l'ID appena inserito del cliente
                int idCliente = Convert.ToInt32(command.ExecuteScalar());


                // Comando per inserire la prenotazione
                var cmd = "INSERT INTO Prenotazione (Anno,DataPrenotazione, IdCliente,SoggiornoInizio,SoggiornoFine,Caparra,Tariffa,Dettagli,IdCamera) VALUES (@anno,@dataPrenotazione, @idCliente,@soggiornoInizio, @soggiornoFine, @caparra, @tariffa, @dettagli, @idCamera)";
                var commandP = new SqlCommand(cmd, conn);
                commandP.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;
                commandP.Parameters.Add("@soggiornoInizio", SqlDbType.DateTime).Value = prenotazione.SoggiornoInizio;
                commandP.Parameters.Add("@soggiornoFine", SqlDbType.DateTime).Value = prenotazione.SoggiornoFine;
                commandP.Parameters.Add("@caparra", SqlDbType.Int).Value = prenotazione.Caparra;
                commandP.Parameters.Add("@tariffa", SqlDbType.NVarChar).Value = prenotazione.Tariffa;
                commandP.Parameters.Add("@dettagli", SqlDbType.NVarChar).Value = prenotazione.Dettagli;
                commandP.Parameters.Add("@idCamera", SqlDbType.Int).Value = prenotazione.IdCamera;
                commandP.Parameters.Add("@dataPrenotazione", SqlDbType.DateTime).Value = DateTime.Now;
                commandP.Parameters.Add("@anno", SqlDbType.Int).Value = (int)DateTime.Now.Year;

                // Esegue il comando di inserimento della prenotazione
                commandP.ExecuteNonQuery();

                conn.Close();
            }


            return View("~/Views/Home/Index.cshtml");
        }



        





        public ActionResult AddServizio(ServizioAggiuntivo servizio)
        {

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Comando per inserire il cliente
                var command = new SqlCommand ("INSERT INTO ServiziRichiesti (IdPrenotazione,Quantita,IdServizio) VALUES ( @idPrenotazione , @quantita, @idServizio)", conn);
                command.Parameters.Add("@idPrenotazione", SqlDbType.Int).Value = (int)servizio.IdPrenotazione;
                command.Parameters.Add("@quantita", SqlDbType.NVarChar).Value = servizio.Quantita;
                command.Parameters.Add("@idServizio", SqlDbType.Int).Value = (int)servizio.IdServizio;


                command.ExecuteNonQuery();

                conn.Close();
            }

            return View("~/Views/Home/Index.cshtml");
        }



    }
}