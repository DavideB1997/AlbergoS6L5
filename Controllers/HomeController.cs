using AlbergoS6L5.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AlbergoS6L5.Controllers
{
    public class HomeController : Controller
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Prenotazione()
        {
            ViewBag.Message = "Pagina prenotazione";

            List<Camera> camere = new List<Camera>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Camere", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var camera = new Camera()
                        {
                            IdCamera = (int)reader["IdCamera"],
                            TipologiaCamera = reader["TipologiaCamera"].ToString(),
                            Descrizione = reader["Descrizione"].ToString(),
                            Prezzo = (int)reader["Prezzo"],
                        };
                        camere.Add(camera);
                    }
                }
            }

            ViewBag.Camere = camere;


            return View("~/Views/Prenotazione/PrenotazioneForm.cshtml");
        }






        public ActionResult CheckOut(int IdPrenotazione)
        {
            int totale = 0;

            ViewBag.Servizi = null;
            ViewBag.CheckOut = null;
            ViewBag.Totale = null;

            List<PrenotazioneCheckOut> checkOut = new List<PrenotazioneCheckOut>();
            List<Servizio> servizi = new List<Servizio>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT Prenotazione.SoggiornoInizio, Prenotazione.IdCamera , Prenotazione.Tariffa, Prenotazione.SoggiornoFine, Prenotazione.Caparra FROM Prenotazione WHERE Prenotazione.IdPrenotazione = @IdPrenotazione", conn);
                var comand = new SqlCommand("SELECT SR.*, SA.Descrizione, SA.Costo FROM ServiziRichiesti SR JOIN ServiziAggiuntivi SA ON SR.IdServizio = SA.IdServizio WHERE SR.IdPrenotazione = @IdPrenotazione", conn);
                var cmd = new SqlCommand("UPDATE Prenotazione SET Saldo = @saldo WHERE IdPrenotazione = @IdPrenotazione", conn);



                command.Parameters.AddWithValue("@IdPrenotazione", IdPrenotazione);
                comand.Parameters.AddWithValue("@IdPrenotazione", IdPrenotazione);
                cmd.Parameters.AddWithValue("@IdPrenotazione", IdPrenotazione);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pernCheckOut = new PrenotazioneCheckOut()
                        {
                            SoggiornoFine = (DateTime)reader["SoggiornoFine"],
                            SoggiornoInizio = (DateTime)reader["SoggiornoInizio"],
                            Tariffa = reader["Tariffa"].ToString(),
                            Stanza = (int)reader["IdCamera"],
                        };

                        checkOut.Add(pernCheckOut);
                        totale += reader.GetInt32(reader.GetOrdinal("Caparra"));
                    }
                }

                using (var reader = comand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var serviziCheckOut = new Servizio()
                        {
                            Descrizione = reader["Descrizione"].ToString(),
                            Quantita = (int)reader["Quantita"],
                            Costo = (int)reader["Costo"],
                        };
                        totale += (reader.GetInt32(reader.GetOrdinal("Quantita")) * reader.GetInt32(reader.GetOrdinal("Costo")));

                        servizi.Add(serviziCheckOut);
                    }
                }


                cmd.Parameters.AddWithValue("@saldo", totale);

                cmd.ExecuteNonQuery();



                ViewBag.Servizi = servizi;
                ViewBag.CheckOut = checkOut;
                ViewBag.Totale = totale;


                conn.Close();
            }

            return View("~/Views/Home/CheckOutDef.cshtml");
        }






        public ActionResult Redirect(int IdPrenotazione)
        {
            return RedirectToAction("CheckOut", new { IdPrenotazione = IdPrenotazione });
        }







        public ActionResult GetCheckOutData()
        {
            List<PrenotazioneOption> prenotazioneLista = new List<PrenotazioneOption>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command2 = new SqlCommand("SELECT Prenotazione.IdPrenotazione AS Prenotazione, Cliente.CF, Cliente.Nome AS NomeCliente, Cliente.Cognome AS CognomeCliente FROM Prenotazione INNER JOIN Cliente ON Prenotazione.IdCliente = Cliente.IdCliente", conn);

                using (var reader = command2.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var prenotazione = new PrenotazioneOption()
                            {
                                IdPrenotazione = (int)reader["Prenotazione"],
                                Nome = reader["NomeCliente"].ToString(),
                                Cognome = reader["CognomeCliente"].ToString(),
                            };
                            prenotazioneLista.Add(prenotazione);
                        }
                    }
                }

                conn.Close();
            }

            ViewBag.prenotazioneLista = prenotazioneLista;
            return View("~/Views/Home/CheckOut.cshtml");
        }

        public ActionResult Servizi()
        {
            List<Servizio> servizoLista = new List<Servizio>();
            List<PrenotazioneOption> prenotazioneLista = new List<PrenotazioneOption>();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var command1 = new SqlCommand("SELECT IdServizio,Descrizione, Costo FROM ServiziAggiuntivi", conn);
                    var command2 = new SqlCommand("SELECT Prenotazione.IdPrenotazione AS Prenotazione, Cliente.CF, Cliente.Nome AS NomeCliente, Cliente.Cognome AS CognomeCliente FROM Prenotazione INNER JOIN Cliente ON Prenotazione.IdCliente = Cliente.IdCliente", conn);

                    using (var reader = command1.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var servizio = new Servizio()
                                {
                                    IdServizio = (int)reader["IdServizio"],
                                    Descrizione = reader["Descrizione"].ToString(),
                                    Costo = (int)reader["Costo"],
                                };
                                servizoLista.Add(servizio);
                            }
                        }
                    }

                    using (var reader = command2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var prenotazione = new PrenotazioneOption()
                                {
                                    IdPrenotazione = (int)reader["Prenotazione"],
                                    Nome = reader["NomeCliente"].ToString(),
                                    Cognome = reader["CognomeCliente"].ToString(),
                                };
                                prenotazioneLista.Add(prenotazione);
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return View("Error" + ex);
            }

            ViewBag.servizoLista = servizoLista;
            ViewBag.prenotazioneLista = prenotazioneLista;



            return View("~/Views/Servizi/Servizi.cshtml");
        }
    }
}