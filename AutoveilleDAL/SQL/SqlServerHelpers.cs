using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using System.Diagnostics;
using Outils;
using Outils.Logging;
using System.Data.SqlClient;


//using Suly.ClientCible.Outils;

namespace AutoveilleDAL.SQL
{
    /// <summary>
    /// regroupement de fonctions 'outils' pour sql server
    /// </summary>
    public class SqlServerHelpers
    {
        public static bool AquireMutexLock(SqlConnection aConn, bool aTransactionIsOwner, string aMutexName)
        {
            try
            {
                var sql = "DECLARE @res int; EXEC @res = sp_getapplock @Resource = @LockName, @LockMode = 'Exclusive', @LockOwner = @Owner,  @LockTimeout = 0, @DbPrincipal = 'public';SELECT @res;";

                var cmd = new SqlCommand(sql, aConn);
                cmd.AddParameterWithValue("@LockName", "ClientCibleApp-" + aMutexName);
                cmd.AddParameterWithValue("@Owner", aTransactionIsOwner ? "Transaction" : "Session");
                var retVal = (int)cmd.ExecuteScalar();
                return retVal >= 0;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure c'est produite lors de l'aquisition du mutex d'exclusion pour la BD");
            }
        }

        public static bool ReleaseMutexLock(SqlConnection aConn, bool aTransactionIsOwner, string aMutexName)
        {
            try
            {
                var sql = "DECLARE @res int; EXEC @res = sp_releaseapplock @Resource = @LockName, @LockOwner = @Owner, @DbPrincipal = 'public';SELECT @res;";

                var cmd = new SqlCommand(sql, aConn);
                
                cmd.AddParameterWithValue("@LockName", "ClientCibleApp-" + aMutexName);
                cmd.AddParameterWithValue("@Owner", aTransactionIsOwner ? "Transaction" : "Session");
                var retVal = (int)cmd.ExecuteScalar();
                return retVal >= 0;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw new ReadableException("Une erreure c'est produite lors du reset du mutex d'exclusion pour la BD");
            }
        }

    }
}
