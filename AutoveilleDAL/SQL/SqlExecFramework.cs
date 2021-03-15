using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

//using System.Data.Entity;

namespace AutoveilleDAL.SQL
{
    /// <summary>
    /// Cette classe fournie des methode qui facilite l'execution de commande
    /// sql dans le context de transactions sql.
    /// </summary>
    public class SqlExecFramework
    {
        private class AbortTransactionException : Exception
        {
        }

        private class ExitBodyException : Exception
        {
        }

        public static void Exit()
        {
            throw new ExitBodyException();
        }

        public static void Rollback()
        {
            throw new AbortTransactionException();
        }

        public delegate void Statement(SqlConnection conn, SqlTransaction trans);

        /// <summary>
        /// Execute des commandes sql dans le context d'une transaction
        /// si aucune transaction n'est specifier, une transaction est cree et
        /// un commit/roll-back est fait a la fin du "Statement" passer en argument
        /// </summary>
        /// <param name="aTransaction"></param>
        /// <param name="aAction"></param>
        public static void SecureExecute( string aConnextionString,SqlTransaction aTransaction, Statement aAction)
        {
            SqlConnection conn = null;
            SqlTransaction trans = null;
            bool success = false;
            try
            {
                if (aTransaction != null)
                {
                    trans = aTransaction;
                    conn = aTransaction.Connection;
                }
                else
                {
                    conn = new SqlConnection(ConnectionHelpers.GetConnectionString(aConnextionString));
                    conn.Open();
                    trans = conn.BeginTransaction();
                }

                // Save stuff to DB
                aAction(conn, trans);

                success = true;
            }
            catch (ExitBodyException)
            {
                success = true;
            }
            catch (AbortTransactionException)
            {
                if (aTransaction != null)
                    throw;
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null)
                        trans.Rollback();
                }
                catch
                {
                }

                throw;
            }
            finally
            {
                if (aTransaction == null)
                {
                    if (trans != null)
                    {
                        if (success)
                            trans.Commit();
                    }
                    if (conn != null)
                        conn.Dispose();
                }
            }

        }

        /// <summary>
        /// Execute des commandes sql dans le context d'une transaction si une transaction a ete 
        /// specifier. Si une transaction n'est pas fournie esn argument, cette fonction 
        /// n'initira pas de transaction.
        /// </summary>
        /// <param name="aTransaction"></param>
        /// <param name="aAction"></param>
        public static void Execute(string aConnextionString,SqlTransaction aTransaction, Statement aAction)
        {
            SqlConnection conn = null;
            try
            {
                if (aTransaction != null)
                {
                    conn = aTransaction.Connection;
                }
                else
                {
                    conn = new SqlConnection(ConnectionHelpers.GetConnectionString(aConnextionString));
                    conn.Open();
                }

                // Save stuff to DB
                aAction(conn, aTransaction);
            }
            catch (ExitBodyException)
            {
            }
            catch (AbortTransactionException)
            {
                if (aTransaction != null)
                    throw;
            }
            finally
            {
                if (aTransaction == null && conn != null)
                    conn.Dispose();
            }
        }



    }
}

