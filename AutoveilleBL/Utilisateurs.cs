using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
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
                                         " (SELECT idusergroupe FROM [UsersGroupeCommerce] WHERE role = 'Gestionnaire') ", conn);
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

        public static string GetRoles(string aUserName)
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            var role = "";
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql =
                        "SELECT id,  role" +
                          "  FROM  dbo.UsersGroupe " +
                          "  WHERE	UserName=@username  " +
                          "   AND role = 'Gestionnaire' " ;

                    var cmd = new SqlCommand(sql, conn);
                    cmd.AddParameterWithValue("@username", aUserName);
                    var u = new UtilisateurSite();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                             role = reader.GetString(1);
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
                                Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(3)),
                                NoCommerce=aNoCommerce,
                                TypeUsager= (UserTypes)Enum.Parse(typeof(UserTypes), reader.GetString(4)),
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
                        "SELECT u.Id, u.Email,  u.Prenom, u.nom, u.role, uc.TypeUsager " +
                            " FROM [usersgroupe] u inner join [UsersGroupeCommerce] uc on u.Id= uc.IdUserGroupe  " +
                        " WHERE username=@user ";

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
                                
                                //Role = (Roles)Enum.Parse(typeof(Roles),reader.GetString(4)),
                                //TypeUsager = (UserTypes)Enum.Parse(typeof(UserTypes), reader.GetString(5)),
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
                        "SELECT ug.id, cm.nocommerce, cm.nomcommerce" +
                        " FROM usersgroupe ug  INNER JOIN usersgroupecommerce ugc ON ug.id=ugc.idusergroupe  " +
                        " INNER JOIN tbcommerces cm ON cm.nocommerce=ugc.nocommerce  OR ugc.nocommerce=0 " +
                        " WHERE ug.username=@user ";
                    //" WHERE ug.username=@user  AND ugc.role & 1>0   ";
                    //Console.Write("Hello world");
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
                                //Role = reader.GetInt32(3),
                                //TypeUsager = reader.GetInt32(4),
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

        public static List<UtilisateurSite> GetAllUtilisateurs()
        {
            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
            var res = new List<UtilisateurSite>();
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT u.Id, u.Email,  u.Prenom, u.nom, u.username, u.Langue, u.role FROM [usersgroupe] u  ";
                    var cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var u = new UtilisateurSite()
                            {
                                UserID = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                LastName = reader.GetString(2),
                                FirstName = reader.GetString(3),
                                UserName = reader.GetString(4),
                                Langue = (Langues)Enum.Parse(typeof(Langues),reader.GetString(5)),
                                Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(6)),
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
