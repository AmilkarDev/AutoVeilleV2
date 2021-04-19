using AutoveilleBL.Models;
using AutoveilleDAL.SQL;
using Outils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL
{
    public class Ventes
    {
        
       public static Evenement GetEvenement(int aIdEvenement,int aNoCommerce)
        {
            try
            {
                var res = new Evenement();
                var nameConnStr = Concessions.GetConnectionString(aNoCommerce);
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("GetEvenement", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@nocommerce", aNoCommerce);
                    cmd.AddParameterWithValue("@id", aIdEvenement);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res.IdEvenement = reader.GetInt32(0);
                            res.DateEvenementDebut = reader.GetNullableDate(1);
                            res.DateEvenementFin = reader.GetNullableDate(2);
                            res.TotalEvenements = reader.GetNullableInt(3) ?? 0;
                            res.DateEvenementDebut = reader.GetDateTime(4);
                            res.Utilisateur = reader.GetString(5);
                            res.NoCommerce = reader.GetInt32(6);
                            res.AppelsPrevusDirEv = reader.GetNullableInt(7) ?? 0;
                            res.AppelsPrevusCASuly = reader.GetNullableInt(8) ?? 0;
                            res.DateModification = reader.GetNullableDate(9);
                            res.UtilisateurModification = reader.GetNullableString(10);
                            res.DatesConfirmer = reader.GetInt32(11);
                        }
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }

        public static List<Relance> GetRelances(int aNoCommerce,int aIdEvenement,int aIdTypeEvenement )
        {
            try
            {
                var res = new List<Relance>();
                var nameConnStr = Concessions.GetConnectionString(aNoCommerce);
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("GetRelances", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@IdEvenement", aIdEvenement);
                    cmd.AddParameterWithValue("@noCommerce", aNoCommerce);
                    cmd.AddParameterWithValue("@TypeRelanceAffichage", aIdTypeEvenement);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var relance = new Relance()
                            {
                                Id = reader.GetInt32(0),
                                Nom = reader.GetNullableString(3),
                                Prenom = reader.GetNullableString(4),
                                Compagnie = reader.GetNullableString(5),
                                TelephoneResidence = reader.GetNullableString(7),
                                TelephoneTravail = reader.GetNullableString(8),
                                Cellulaire = reader.GetNullableString(9),
                                ExtTravail = reader.GetNullableString(10),
                                FinDuTerme = reader.GetNullableDate(18),
                                Modele = reader.GetNullableString(12),
                                Annee = reader.GetNullableInt(13),
                            };
                            res.Add(relance);
                        }
                    }

                });
                return res;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }
        public static Evenement GetProchaineEvenement(int aNoCommerce)
        {
            try
            {
                var res = new Evenement();
                var nameConnStr = Concessions.GetConnectionString(aNoCommerce);
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
               {
                   var sql = new StringTemplate(".sql").LoadAndFill("GetProchaineEvenement", StringTemplateOptions.TrimBlanks, new { });
                   var cmd = new SqlCommand(sql, conn, trans);
                   cmd.AddParameterWithValue("@nocommerce", aNoCommerce);

                   using (var reader = cmd.ExecuteReader())
                   {
                       if (reader.Read())
                       {
                           res.IdEvenement = reader.GetInt32(0);
                           res.DateEvenementDebut = reader.GetNullableDate(1);
                           res.DateEvenementFin = reader.GetNullableDate(2);
                           res.TotalEvenements = reader.GetNullableInt(3) ?? 0;
                           res.DateEvenementDebut = reader.GetDateTime(4);
                           res.Utilisateur = reader.GetString(5);
                           res.NoCommerce = reader.GetInt32(6);
                           res.AppelsPrevusDirEv = reader.GetNullableInt(7) ?? 0;
                           res.AppelsPrevusCASuly = reader.GetNullableInt(8) ?? 0;
                           res.DateModification = reader.GetNullableDate(9);
                           res.UtilisateurModification = reader.GetNullableString(10);
                           res.DatesConfirmer = reader.GetInt32(11);
                       }
                   }
               });
                return res;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }

        public static List<EvenementAutoveille> GetVentes(int aNoCommerce, DateTime? aDateFrom = null)
        {
            var evenementAutoveille = new List<EvenementAutoveille>();
            var nameConnStr = Concessions.GetConnectionString(aNoCommerce);
            string connStr = ConnectionHelpers.GetConnectionString(nameConnStr);
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    DateTime dateFrom = aDateFrom ?? DateTime.Now;
                    string sql = "SELECT e.Id ,DateEvenementDebut,DateEvenementFin ,AppelsPrevusDirEv  " +
                                " ,AppelsPrevusCASuly ,e.NoCommerce ,DateCreation ,Utilisateur, " +
                                " sum(case when ra.Type=1 then 1 else 0 end) NbreAppelsEquite, " +
                                " sum(case when ra.Type=2 then 1 else 0 end) NbreAppelsLocationEquite, " +
                                " sum(case when ra.Type=3 then 1 else 0 end) NbreAppelsLocation, " +
                                " sum(case when ra.Type=4 then 1 else 0 end) NbreAppelsFinancementTaktic, " +
                                " sum(case when ra.Type=5 then 1 else 0 end) NbreAppelsServiceTaktic, " +
                                " sum(case when ra.Type=7 then 1 else 0 end) NbreAppelsWalkOut, " +
                                " sum(case when ra.Type=8 then 1 else 0 end) NbreAppelsLeads, " +
                                " sum(case when ra.Type=9 then 1 else 0 end) NbreAppelsRDVService, " +
                                " sum(case when ra.Type=10 then 1 else 0 end) NbreAppelsFinancementTakticLandingPage, " +
                                " sum(case when ra.Type=11 then 1 else 0 end) NbreAppelsServiceTakticLandingPage " +
                                " FROM atv.TbEvenement  e INNER JOIN " +
                                " atv.TbRelanceAutoveille ra on e.Id=ra.IdEvenement " +
                                 " WHERE e.nocommerce=@noCommmerce AND DateEvenementFin>=@dateFrom " +
                                 " group by e.id,DateEvenementDebut,DateEvenementFin ,AppelsPrevusDirEv  " +
                                 " ,AppelsPrevusCASuly ,e.NoCommerce ,DateCreation ,Utilisateur ";

                    var cmd = new SqlCommand(sql, conn);

                    cmd.AddParameterWithValue("@noCommmerce", aNoCommerce);
                    cmd.AddParameterWithValue("@dateFrom", dateFrom);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var evenement = new EvenementAutoveille()
                            {
                                IdEvenement = reader.GetInt32(0),
                                DateEvenementDebut = reader.GetDateTime(1),
                                DateEvenementFin = reader.GetDateTime(2),
                                AppelsPrevusDirEv = reader.GetInt32(3),
                                AppelsPrevusCASuly = reader.GetInt32(4),
                                NoCommerce = reader.GetInt32(5),
                                //DateCreation = reader.GetDateTime(6),
                                //Utilisateur = reader.GetString(7),
                                NbreAppelsEquite = reader.GetInt32(8),
                                NbreAppelsLocationEquite = reader.GetInt32(9),
                                NbreAppelsLocation = reader.GetInt32(10),
                                NbreAppelsFinancementTaktic = reader.GetInt32(11),
                                NbreAppelsServiceTaktic = reader.GetInt32(12),
                                NbreAppelsWalkOut = reader.GetInt32(13),
                                NbreAppelsLeads = reader.GetInt32(14),
                                NbreAppelsFinancementTakticLandingPage = reader.GetInt32(14),
                                NbreAppelsServiceTakticLandingPage = reader.GetInt32(15),
                            };
                            evenementAutoveille.Add(evenement);
                        }
                        return evenementAutoveille;
                    }
                }
                return evenementAutoveille;
            }

            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }

        }
        public static List<Evenement> GetEvenementsByConsultant(int aUserId)
        {
            var listeEvenements = new List<Evenement>();
            //string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                SqlExecFramework.Execute("AutoveilleMain", null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("GetEvenementsByConsultant", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@idUser", aUserId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var evenement = new Evenement()
                            {
                                IdEvenement = reader.GetInt32(0),
                                NoCommerce = reader.GetInt32(1),
                            };
                            listeEvenements.Add(evenement);
                        }
                    };
                });
                return listeEvenements;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la recherche des evenements pour le consultant: " + aUserId.ToString());

            }
        }
        public static List<ListeAppels> GetAppelsConsultantById(int aId, int aNoCommerce)
        {
            var listeListeAppels = new List<ListeAppels>();
            var nameConnStr = Concessions.GetConnectionString(aNoCommerce);
            //string connStr = ConnectionHelpers.GetConnectionString(nameConnStr);
            try
            {
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("GetAppelsConsultantById", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@id", aId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var listeAppels = new ListeAppels()
                            {
                                IdEvenement = aId,
                                NoCommerce= aNoCommerce,
                                TypeAppel = reader.GetInt32(0),
                                DescriptionListe = reader.GetString(3),
                                OrdreAfichagePortail =reader.GetInt32(4),
                                NbreAppels=reader.GetInt32(5)
                            };
                            listeListeAppels.Add(listeAppels);
                        }                       
                    };
                });
                return listeListeAppels;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste des appels pour le consultant.");
            }
        }
    }       
}
