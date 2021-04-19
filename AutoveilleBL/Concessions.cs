using AutoveilleBL.Models;
using AutoveilleBL.Models.Enumerations;
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
    public class Concessions
    {
        public static List<Concession> GetConcessionsActiveAutoveilleV2()
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                var res=new List<Concession>();
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT Nocommerce,nomcommerce FROM tbcommerces  " +
                        " WHERE actif=1";

                    var cmd = new SqlCommand(sql, conn);
      
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            var concession=new Concession()
                                {
                                    NoCommerce=reader.GetInt32(0),
                                    NomCommerce=reader.GetNullableString(1)??"",
                                };
                            res.Add(concession );
                        }
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }

        public static string GetConnectionString(int aNoCommerce)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                var res = "";
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT ConnexionString FROM tbcommerces  " +
                        " WHERE nocommerce=@noCommerce";

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@noCommerce", aNoCommerce);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            res = reader.GetString(0);
                        }
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }

        public static List<EvenementAutoveille> GetEvenements(int aNoCommerce, DateTime aDateFrom)
        {
            var nameConnStr = GetConnectionString(aNoCommerce);
            string connStr = ConnectionHelpers.GetConnectionString(nameConnStr);
            try
            {
                var res = new List<EvenementAutoveille>();
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT Id ,DateEvenementDebut,DateEvenementFin ,AppelsPrevusDirEv " + 
                                " ,AppelsPrevusCASuly ,NoCommerce ,DateCreation ,Utilisateur " +
                                "  FROM atv.TbEvenement  " +
                                 " WHERE nocommerce=@noCommmerce AND DateEvenementFin>=@dateFrom ";

                    var cmd = new SqlCommand(sql, conn);

                    cmd.AddParameterWithValue("@noCommmerce", aNoCommerce);
                    cmd.AddParameterWithValue("@dateFrom", aDateFrom);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var evenement = new EvenementAutoveille()
                            {
                                IdEvenement = reader.GetInt32(0),
                                DateEvenementDebut =reader.GetDateTime(1),
                                DateEvenementFin =reader.GetDateTime(2),
                                AppelsPrevusDirEv =reader.GetInt32(3),
                                AppelsPrevusCASuly =reader.GetInt32(4),
                                NoCommerce = reader.GetInt32(5),
                            };
                            res.Add(evenement);
                        }
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }       
     public static string GetNomCommerce(int aNoCommerce)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                var res = "";
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT NomCommerce FROM tbcommerces  " +
                        " WHERE nocommerce=@noCommerce";

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@noCommerce", aNoCommerce);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            res = reader.GetString(0);
                        }
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la recherche de nom du commerce.");
            }
        }

    }
}
