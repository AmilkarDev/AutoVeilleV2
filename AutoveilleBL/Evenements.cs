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
    public static class Evenements
    {

        public static Evenement GetEvenement(int aIdEvenement, int aNoCommerce)
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
                            res.Actif = reader.GetBoolean(12);
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


        public static List<Evenement> GetAllEvenements(int aNoCommerce)
        {
            try
            {
                var eventsList = new List<Evenement>();
                var res = new Evenement();
                var nameConnStr = Concessions.GetConnectionString(aNoCommerce);

                //var nameConnStr = ConnectionHelpers.GetConnectionString("AutoveilleMain"); 
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("GetAllEvenements", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@nocommerce", aNoCommerce);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                res = new Evenement();
                                res.IdEvenement = reader.GetInt32(0);
                                res.DateEvenementDebut = reader.GetNullableDate(1);
                                res.DateEvenementFin = reader.GetNullableDate(2);
                                res.TotalEvenements = reader.GetNullableInt(3) ?? 0;
                                res.DateCreation = reader.GetDateTime(4);
                                res.Utilisateur = reader.GetString(5);
                                res.NoCommerce = reader.GetInt32(6);
                                res.AppelsPrevusDirEv = reader.GetNullableInt(7) ?? 0;
                                res.AppelsPrevusCASuly = reader.GetNullableInt(8) ?? 0;
                                res.DateModification = reader.GetNullableDate(9);
                                res.UtilisateurModification = reader.GetNullableString(10);
                                res.DatesConfirmer = reader.GetInt32(11);
                                res.Actif = reader.GetBoolean(12);
                                eventsList.Add(res);
                            }
                        }
                    }
                });
                return eventsList;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
        }
        public static string HandleDate(DateTime? date)
        {
            return date == null ? null : date.Value.ToString("yyyy-MM-dd");
        }
        public static int InsertEvenement(Evenement evenement)
        {
            string connStr = ConnectionHelpers.GetConnectionString("Autoveille");
            //Create the SQL Query for inserting an event
            string sqlQuery = String.Format("Insert into atv.TbEvenement (DateEvenementDebut, DateEvenementFin ,TotalEvenements, " +
                "DateCreation,Utilisateur,NoCommerce   ,AppelsPrevusDirEv  ,AppelsPrevusCASuly  ,DateModification ,UtilisateurModification " +
                ", DatesConfirmer, Actif) Values('{0}', '{1}', {2}, '{3}','{4}',{5},{6}, {7},'{8}', '{9}', {10}, {11});"
            + "Select @@Identity",
            HandleDate(evenement.DateEvenementDebut), HandleDate(evenement.DateEvenementFin)

            , evenement.TotalEvenements,
            evenement.DateCreation.ToString("yyyy-MM-dd"), evenement.Utilisateur, evenement.NoCommerce, evenement.AppelsPrevusDirEv, evenement.AppelsPrevusCASuly
            , HandleDate(evenement.DateModification)

            , evenement.UtilisateurModification, evenement.DatesConfirmer, 1);

            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            //Create a Command object
            SqlCommand command = new SqlCommand(sqlQuery, connection);

            //Execute the command to SQL Server and return the newly created ID
            int newEventID = Convert.ToInt32((decimal)command.ExecuteScalar());

            //Close and dispose
            command.Dispose();
            connection.Close();
            connection.Dispose();

            // Set return value
            return newEventID;
        }

        public static int SaveEvenement(Evenement evenement)
        {
            string connStr = ConnectionHelpers.GetConnectionString("Autoveille");
            //Create the SQL Query for inserting an event
            //Create the SQL Query for inserting an event
            string createQuery = String.Format("Insert into atv.TbEvenement (DateEvenementDebut, DateEvenementFin ,TotalEvenements, " +
                "DateCreation,Utilisateur,NoCommerce   ,AppelsPrevusDirEv  ,AppelsPrevusCASuly  ,DateModification ,UtilisateurModification " +
                ", DatesConfirmer) Values('{0}', '{1}', {2}, '{3}','{4}',{5},{6}, {7},'{8}', '{9}', {10} ,{11}   );"
            + "Select @@Identity",
            HandleDate(evenement.DateEvenementDebut), HandleDate(evenement.DateEvenementFin), evenement.TotalEvenements,
            evenement.DateCreation.ToString("yyyy-MM-dd"), evenement.Utilisateur, evenement.NoCommerce, evenement.AppelsPrevusDirEv, evenement.AppelsPrevusCASuly
            , HandleDate(evenement.DateModification), evenement.UtilisateurModification, evenement.DatesConfirmer, evenement.Actif ? 1 : 0);


            //Create the SQL Query for updating an event
            string updateQuery = String.Format("Update atv.TbEvenement SET DateEvenementDebut='{0}', DateEvenementFin = '{1}', TotalEvenements ={2}," +
                " DateCreation = '{3}',Utilisateur='{4}', NoCommerce ={5},AppelsPrevusDirEv={6},AppelsPrevusCASuly={7},DateModification='{8}' ," +
                "UtilisateurModification='{9}',DatesConfirmer={10}, Actif = {11} " +
                "Where Id = {12};",
                HandleDate(evenement.DateEvenementDebut), HandleDate(evenement.DateEvenementFin), evenement.TotalEvenements,
            evenement.DateCreation.ToString("yyyy-MM-dd"), evenement.Utilisateur, evenement.NoCommerce, evenement.AppelsPrevusDirEv, evenement.AppelsPrevusCASuly
            , HandleDate(evenement.DateModification), evenement.UtilisateurModification, evenement.DatesConfirmer, evenement.Actif ? 1 : 0, evenement.IdEvenement);

            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            //Create a Command object
            SqlCommand command = null;

            if (evenement.IdEvenement != 0)
                command = new SqlCommand(updateQuery, connection);
            else
                command = new SqlCommand(createQuery, connection);

            int savedEventID = 0;
            try
            {
                //Execute the command to SQL Server and return the newly created ID
                var commandResult = command.ExecuteScalar();
                if (commandResult != null)
                {
                    savedEventID = Convert.ToInt32(commandResult);
                }
                else
                {
                    //the update SQL query will not return the primary key but if doesn't throw exception 
                    //then we will take it from the already provided data
                    savedEventID = evenement.IdEvenement;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }

            //Close and dispose
            command.Dispose();
            connection.Close();
            connection.Dispose();

            return savedEventID;
        }


        public static bool DeleteEvenement(Evenement evenement)
        {
            bool result = false;
            string connStr = ConnectionHelpers.GetConnectionString("Autoveille");
            try
            {
                var eventsList = new List<Evenement>();
                var res = new Evenement();
                var nameConnStr = Concessions.GetConnectionString(evenement.NoCommerce);

                //var nameConnStr = ConnectionHelpers.GetConnectionString("AutoveilleMain"); 
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("deleteEvenement", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@nocommerce", evenement.NoCommerce);
                    cmd.AddParameterWithValue("@idEvenement", evenement.IdEvenement);

                    int rowsDeletedCount = cmd.ExecuteNonQuery();
                    if (rowsDeletedCount != 0)
                        result = true;
                });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
            }
            return result;
        }
    }
}
