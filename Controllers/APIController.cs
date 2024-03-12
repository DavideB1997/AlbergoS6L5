using AlbergoS6L5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AlbergoS6L5.Controllers
{
    public class APIController : ApiController
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();


        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        [HttpGet]
        [Route("prenotazione/{id}")]
        public IHttpActionResult GetById(string id)
        {
            List<PrenotazioneCheckOut> prenotazioni = new List<PrenotazioneCheckOut>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT Prenotazione.SoggiornoInizio, Prenotazione.IdCamera , Prenotazione.Tariffa, Prenotazione.SoggiornoFine, Prenotazione.Caparra FROM Prenotazione WHERE Prenotazione.IdPrenotazione = @IdPrenotazione", conn);
                command.Parameters.AddWithValue("@IdPrenotazione", id);

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

                        prenotazioni.Add(pernCheckOut);
                    }
                }
            }

            // Serializza la lista di prenotazioni in un formato JSON e restituisci come IHttpActionResult
            return Ok(prenotazioni);
        }

        [HttpGet]
        [Route("tariffa/{id}")]
        public IHttpActionResult GetByTariffa(int id)
        {
            List<PrenotazioneCheckOut> prenotazioni = new List<PrenotazioneCheckOut>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT Prenotazione.SoggiornoInizio, Prenotazione.IdCamera , Prenotazione.Tariffa, Prenotazione.SoggiornoFine, Prenotazione.Caparra FROM Prenotazione WHERE Prenotazione.Tariffa = @Id", conn);
                command.Parameters.AddWithValue("@Id", id);

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

                        prenotazioni.Add(pernCheckOut);
                    }
                }
            }

            // Serializza la lista di prenotazioni in un formato JSON e restituisci come IHttpActionResult
            return Ok(prenotazioni);
        }


        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}