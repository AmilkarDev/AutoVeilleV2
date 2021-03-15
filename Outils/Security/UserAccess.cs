using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security;



namespace Outils.Security
{
    public class UserAccess
    {
        public static UserSecurityInfo DefaultUser { get; set; }

        static public void LoginUser( string name, string psw)
        {
            try
            {
                DefaultUser = ValidateCredentials(name, psw);
            }
            catch (Exception)
            {
                DefaultUser = null;
                Trace.TraceError( String.Format("Failed to login user: {0}", name) );
                throw;
            }
        }

        static public UserSecurityInfo ValidateCredentials(string name, string psw)
        {
            Trace.TraceInformation(String.Format("user {0} attempting to login to system", name));
            var conStr = ConnectionHelpers.GetConnectionString(name, psw);

            try
            {
                var info = new UserSecurityInfo() { Name = name, Password = psw };
                using (var conn = new SqlConnection(conStr))
                {
                    conn.Open();
                }

                return info;
            }
            catch (Exception ex)
            {
                Trace.TraceError(String.Format("Failed to validate credentials for user: {0}\n {1}", name, ex.Message));
                throw new SecurityException( "Accés refusé!" );
            }
        }

    }
}
