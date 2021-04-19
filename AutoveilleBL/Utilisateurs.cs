using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoveilleDAL;
using AutoveilleDAL.SQL;
using Outils;
using AutoveilleBL.Models.Web;
using System.Diagnostics;

namespace AutoveilleBL
{
    public static class Utilisateurs
    {

     
        public static bool ValidateUtilisateurGroupe(string userName, string passWord)
        {
            SqlCommand cmd;
            string lookupPassword = null;

            try
            {
                SqlExecFramework.Execute("AutoveilleMain", null, (conn, trans) =>
                {
                    cmd = new SqlCommand("SELECT [Password] FROM usersGroupe WHERE username=@user AND id in " +
                                         " (SELECT idusergroupe FROM [UsersGroupeCommerce] WHERE role & 1>0) ", conn);
                    cmd.AddParameterWithValue("@user", userName);

                    lookupPassword = (string)cmd.ExecuteScalar();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                // Ajoute ici la gestion des erreurs pour le débogage.
                // Ce message d'erreur ne doit pas être renvoyé à l'appelant.
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
            }

            // Si aucun mot de passe n'est trouvé, retourne la valeur false.
            if (null == lookupPassword)
            {
                // Vous pouvez écrire ici les échecs de tentative de connexion dans le journal des événements, pour une sécurité accrue.
                return false;
            }
            passWord = Encryption.EncryptRijndael(passWord, Cle.SaltSite, Cle.KeySite, Cle.IterationEncryptionSite);
            // Compare lookupPassword et le passWord d'entrée à l'aide d'une comparaison sensible à la casse.
            return (0 == string.Compare(lookupPassword, passWord, false));

        }

        public static int GetRoles(string aUserName)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            var role = 0;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql =
                        "SELECT id,  role" +
                          "  FROM  dbo.UsersGroupe " +
                          "  WHERE	UserName=@username  " +
                          "   AND role & 1>0 " ;

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@username", aUserName);
                    var u = new UtilisateurSite();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                             role = reader.GetInt32(1);
                        }
                    }
                    return role;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure est survenue à la génération de la liste des concessions", ex);
            }
        }

        public static UtilisateurSite GetRoles(string aUserName, int aNoCommerce)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql =
                        "SELECT IdUserGroupe, titre, nocommerce, role, TypeUsager, uc.id " + 
                          "  FROM dbo.UsersGroupeCommerce uc INNER JOIN dbo.UsersGroupe u ON "+
	                           " uc.IdUserGroupe=u.Id" +
                          "  WHERE	UserName=@username  " +
                          "  AND (nocommerce=@nocommerce " +
                         "   OR nocommerce=0) AND role & 1>0 " +
                          "  ORDER BY nocommerce desc   " ;

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@username", aUserName);
                    cmd.AddParameterWithValue("@nocommerce", aNoCommerce);
                    var u = new UtilisateurSite();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            u = new UtilisateurSite
                            {
                                UserID = reader.GetInt32(0),
                                Role = reader.GetInt32(3),
                                NoCommerce=aNoCommerce,
                                TypeUsager=reader.GetInt32(4),
                                UserName = aUserName,
                                //Role = reader.GetInt32(6),
                                //TypeUsager = reader.GetInt32(7),
                            };

                        }
                    }
                    return u;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure est survenue à la génération de la liste des concessions", ex);
            }
        }

        public static UtilisateurSite GetUtilisateurFromAlias(string aUserName)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql =
                        "SELECT id, email,  u.Prenom, u.nom " + 
                            " FROM usersgroupe u   " +
                        " WHERE username=@user  AND  id in (SELECT idusergroupe FROM [UsersGroupeCommerce] WHERE role & 1>0)  ";

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@user", aUserName);

                    var u = new UtilisateurSite();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            u = new UtilisateurSite
                            {
                                UserID = reader.GetInt32(0),
                                Email = reader.GetNullableString(1),
                                //NoCommerce = reader.GetInt32(2),
                                //NomCommerce = reader.GetNullableString(3),
                                FirstName = reader.GetNullableString(2),
                                LastName = reader.GetNullableString(3),
                                UserName = aUserName,
                                //Role = reader.GetInt32(6),
                                //TypeUsager = reader.GetInt32(7),
                            };

                        }
                    }
                    return u;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure est survenue à la génération de la liste des concessions", ex);
            }
        }

        public static List<UtilisateurSiteCommerces> GetUtilisateurCommerceFromAlias(string aUserName)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            var res = new List<UtilisateurSiteCommerces>();
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql =
                        "SELECT ug.id, cm.nocommerce, cm.nomcommerce, ugc.role, typeusager  " +
                        " FROM usersgroupe ug  INNER JOIN usersgroupecommerce ugc ON ug.id=ugc.idusergroupe  " +
                        " INNER JOIN tbcommerces cm ON cm.nocommerce=ugc.nocommerce  OR ugc.nocommerce=0 " +
                        " WHERE ug.username=@user  AND ugc.role & 1>0   ";

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@user", aUserName);



                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var u = new UtilisateurSiteCommerces()
                            {
                                UserId = reader.GetInt32(0),
                                NoCommerce = reader.GetInt32(1),
                                NomCommerce = reader.GetNullableString(2),
                                Role = reader.GetInt32(3),
                                TypeUsager = reader.GetInt32(4),
                            };
                            res.Add(u);
                        }
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure est survenue à la génération de la liste des concessions", ex);
            }
        }


    }
}
