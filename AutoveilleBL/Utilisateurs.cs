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
            //passWord = Encryption.EncryptRijndael(passWord, Cle.Salt, Cle.Key, Cle.IterationEncryption);
            //string ss = Encryption.DecryptRijndael(passWord, Cle.SaltSite, Cle.KeySite, Cle.IterationEncryptionSite);
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
                          "   AND role = 'Gestionnaire' ";

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

        public static bool DeleteUtilisateur(UtilisateurSite user)
        {
            bool result = false;
            try
            {
                var nameConnStr = "AutoveilleMain";
                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("deleteUtilisateur", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@idUtilisateur", user.UserID);

                    int rowsDeletedCount = cmd.ExecuteNonQuery();
                    if (rowsDeletedCount != 0)
                        result = true;
                });

                SqlExecFramework.Execute(nameConnStr, null, (conn, trans) =>
                {
                    var sql = new StringTemplate(".sql").LoadAndFill("deleteUtilisateurConcession", StringTemplateOptions.TrimBlanks, new { });
                    var cmd = new SqlCommand(sql, conn, trans);
                    cmd.AddParameterWithValue("@idUtilisateur", user.UserID);

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

        public static int SaveUtilisateur(UtilisateurSite user)
        {
            int savedUserID = 0;

            if (user != null && !string.IsNullOrWhiteSpace(user.Password) && user.Password.Equals(user.ConfirmPassword))
            {
                string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
                //Encrypt password
                string password = Encryption.EncryptRijndael(user.Password, Cle.SaltSite, Cle.KeySite, Cle.IterationEncryptionSite);

                //Create and open a connection to SQL Server 
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    //Create the SQL Query for updating an event
                    string updateQuery = "Update [AutoveilleMain].dbo.UsersGroupe SET UserName = @UserName, Password = @Password, Nom = @Nom, Prenom = @Prenom, Email = @Email, Langue = @Langue, Role= @Role Where Id = @Id;";

                    //Create a Command object
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Nom", user.FirstName);
                        command.Parameters.AddWithValue("@Prenom", user.LastName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Langue", user.Langue.ToString());
                        command.Parameters.AddWithValue("@Role", user.Role.ToString());
                        command.Parameters.AddWithValue("@Id", user.UserID);
                        try
                        {
                            var commandResult = command.ExecuteNonQuery();

                            savedUserID = user.UserID;
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.ToString());
                            throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
                        }
                    }

                    updateQuery = "UPDATE  [AutoveilleMain].[dbo].[UsersGroupeCommerce] set nocommerce = @nocommerce, Titre = @Titre where IdUserGroupe = @IdUserGroupe;";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nocommerce", user.NoCommerce);
                        command.Parameters.AddWithValue("@Titre", user.FirstName);
                        command.Parameters.AddWithValue("@IdUserGroupe", user.UserID);

                        try
                        {
                            var commandResult = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError(ex.ToString());
                            throw new ReadableException("Une erreures c'est produite lors de la generation de la liste de concessions active.");
                        }
                    }
                }
            }
            return savedUserID;
        }
        public static int InsertUtilisateurCommerce(UtilisateurSiteCommerces utilisateurSiteCommerces)
        {
            int newUserGroupCommercesId = 0;

            string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");

            string sqlQuery = "INSERT INTO dbo.UsersGroupeCommerce (IdUserGroupe,Titre,nocommerce,TypeUsager) values (@IdUserGroupe,@Titre,@nocommerce,@TypeUsager);" +
                                "Select @@Identity;";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdUserGroupe", utilisateurSiteCommerces.UserId);
                    command.Parameters.AddWithValue("@Titre", utilisateurSiteCommerces.Titre);
                    command.Parameters.AddWithValue("@nocommerce", utilisateurSiteCommerces.NoCommerce);
                    command.Parameters.AddWithValue("@TypeUsager", utilisateurSiteCommerces.TypeUsager.ToString());

                    newUserGroupCommercesId = Convert.ToInt32((decimal)command.ExecuteScalar());
                }
            }

            return newUserGroupCommercesId;
        }
        public static int InsertUtilisateur(UtilisateurSite user)
        {
            int newUserId = 0;

            if (user != null && !string.IsNullOrWhiteSpace(user.Password) && user.Password.Equals(user.ConfirmPassword))
            {
                string connStr = ConnectionHelpers.GetConnectionString("AutoveilleMain");
                //Encrypt password
                string password = Encryption.EncryptRijndael(user.Password, Cle.SaltSite, Cle.KeySite, Cle.IterationEncryptionSite);

                //Create the SQL Query for inserting an user
                string sqlQuery = String.Format("Insert into dbo.UsersGroupe (UserName,Password,Nom,Prenom,Email,Langue,Role) " +
                                                    "Values('{0}','{1}', '{2}','{3}','{4}','{5}','{6}'); " +
                                                    "Select @@Identity",
                    user.UserName, password, user.FirstName, user.LastName, user.Email, user.Langue.ToString(), user.Role.ToString());

                //Create and open a connection to SQL Server 
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    //Create a Command object
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //Execute the command to SQL Server and return the newly created ID
                        newUserId = Convert.ToInt32((decimal)command.ExecuteScalar());
                    }

                }

                // Create and Insert UserGroupCommerce
                if (newUserId != 0)
                {
                    user.UserID = newUserId;

                    UtilisateurSiteCommerces utilisateurSiteCommerces = new UtilisateurSiteCommerces()
                    {
                        NoCommerce = user.NoCommerce,
                        Role = user.Role,
                        NomCommerce = user.NomCommerce,
                        Titre = user.FirstName,
                        TypeUsager = UserTypes.Suly,
                        UserId = user.UserID
                    };
                    int newUserGroupCommercesId = InsertUtilisateurCommerce(utilisateurSiteCommerces);
                }
            }

            // Set return value
            return newUserId;
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
                          "  FROM dbo.UsersGroupeCommerce uc INNER JOIN dbo.UsersGroupe u ON " +
                               " uc.IdUserGroupe=u.Id" +
                          "  WHERE	UserName=@username  " +
                          "  AND (nocommerce=@nocommerce " +
                         "   OR nocommerce=0) AND role & 1>0 " +
                          "  ORDER BY nocommerce desc   ";

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
                                NoCommerce = aNoCommerce,
                                TypeUsager = (UserTypes)Enum.Parse(typeof(UserTypes), reader.GetString(4)),
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

                    string sql = "SELECT u.Id, u.Email,  u.Prenom, u.nom, u.username, u.Langue, u.role, uc.nocommerce FROM dbo.usersgroupe u left join dbo.UsersGroupeCommerce uc on u.Id = uc.IdUserGroupe;";
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
                                Langue = (Langues)Enum.Parse(typeof(Langues), reader.GetString(5)),
                                Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(6)),
                                NoCommerce = reader.GetInt32(7)
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
