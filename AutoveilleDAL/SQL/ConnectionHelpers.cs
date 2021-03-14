using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using Outils;



namespace AutoveilleDAL.SQL
{
    /// <summary>
    /// classe regoupe des fonctions de type "helper" pour ce connecter a 
    /// un serveur de DB.
    /// </summary>
    public class ConnectionHelpers
    {
        /// <summary>
        /// nom de la connection par default, dans app.config/web.config
        /// </summary>
        private const string DefaultConnectionStringName = "mainConnection";

        /// <summary>
        /// Retourne une "connection string" complete, base sur la connection
        /// par default definit dans le fichier app.config ou web.config.
        /// Ajoute l'id du user et le mot de passe du user par default.
        /// </summary>
        /// <returns></returns>
        public static String GetConnectionString()
        {
            try
            {
                var connectionSettings = ConfigurationManager.ConnectionStrings[DefaultConnectionStringName];
                var csb = new SqlConnectionStringBuilder(connectionSettings.ConnectionString);
                var password = Encryption.DecryptRijndael(csb.Password, Cle.Salt, Cle.Key, Cle.IterationEncryption);
                csb.Password = password;

                return csb.ToString();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }

        public static String GetConnectionString(string aConnextionString)
        {
            try
            {
                if (aConnextionString == "")
                {
                    aConnextionString = DefaultConnectionStringName;
                }
                var connectionSettings = ConfigurationManager.ConnectionStrings[aConnextionString];
                var csb = new SqlConnectionStringBuilder(connectionSettings.ConnectionString);
                var password = Encryption.DecryptRijndael(csb.Password, Cle.Salt, Cle.Key, Cle.IterationEncryption);
                csb.Password = password;

                return csb.ToString();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }

        public static String GetConnectionString(string aConnextionString, string aDataSource)
        {
            try
            {
                if (aConnextionString == "")
                {
                    aConnextionString = DefaultConnectionStringName;
                }

                //var connectionSettings = ConfigurationManager.ConnectionStrings[DefaultConnectionStringName];
                //var csb = new SqlConnectionStringBuilder(connectionSettings.ConnectionString);
                //var password = Encryption.DecryptRijndael(csb.Password, Cle.Salt, Cle.Key, Cle.IterationEncryption);
                //csb.Password = password;

                var connectionSettings = ConfigurationManager.ConnectionStrings[aConnextionString];
                var csb = new SqlConnectionStringBuilder(connectionSettings.ConnectionString);
                var password = Encryption.DecryptRijndael(csb.Password, Cle.Salt, Cle.Key, Cle.IterationEncryption);
                csb.Password = password;

                csb.DataSource = aDataSource;

                return csb.ToString();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }

    }
}
