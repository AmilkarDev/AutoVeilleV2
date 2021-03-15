using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
//using System.Data.SqlClient;

namespace AutoveilleDAL.SQL
{
    public static class SqlClientExtensions
    {
        public static void AddParameterWithValue<T>(this SqlCommand aCmd, String aId, T aValue)
        {
            if (typeof(T) == typeof(String))
                aCmd.Parameters.Add(aId, SqlDbType.VarChar);
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(int?) || typeof(T) == typeof(Int16?))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime?))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(bool?))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(bool))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(Guid))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);
            else if (typeof(T) == typeof(Guid?))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);


            else if (typeof(T).IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        typeof(T).GetGenericArguments()[0].IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else
            {
                throw new Exception("Type inconnu: " + typeof(T));
            }

            if (aValue == null)
                aCmd.Parameters[aId].Value = DBNull.Value;
            else
                aCmd.Parameters[aId].Value = aValue;
        }

        public static void AddParameterWithValue(this SqlCommand aCmd, String aId, Object aValue)
        {
            if (aValue == null || aValue.GetType() == typeof(System.DBNull))
            {
                aCmd.Parameters.Add(aId, SqlDbType.VarChar);
                aCmd.Parameters[aId].Value = DBNull.Value;
                return;
            }

            var type = aValue.GetType();
            if (type == typeof(String))
                aCmd.Parameters.Add(aId, SqlDbType.VarChar);
            else if (type == typeof(int) || type == typeof(Int16))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (type == typeof(DateTime))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (type == typeof(float))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (type == typeof(Double))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (type == typeof(int?) || type == typeof(Int16?))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (type == typeof(DateTime?))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (type == typeof(float?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (type == typeof(Double?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (type == typeof(bool?))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (type == typeof(bool))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (type == typeof(Guid))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);
            else if (type == typeof(Guid?))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);


            else if (type.IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        type.GetGenericArguments()[0].IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else
            {
                throw new Exception("Type inconnu: " + type);
            }

            aCmd.Parameters[aId].Value = aValue;
        }


        public static void AddParameterWithValue<T>(this OleDbCommand aCmd, String aId, T aValue)
        {
            if (typeof(T) == typeof(String))
                aCmd.Parameters.Add(aId, SqlDbType.VarChar);
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(int?) || typeof(T) == typeof(Int16?))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime?))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(bool?))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(bool))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(Guid))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);
            else if (typeof(T) == typeof(Guid?))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);


            else if (typeof(T).IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        typeof(T).GetGenericArguments()[0].IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else
            {
                throw new Exception("Type inconnu: " + typeof(T));
            }

            if (aValue == null)
                aCmd.Parameters[aId].Value = DBNull.Value;
            else
                aCmd.Parameters[aId].Value = aValue;
        }

        public static void AddParameterWithValue<T>(this OdbcCommand aCmd, String aId, T aValue)
        {
            if (typeof(T) == typeof(String))
                aCmd.Parameters.Add(aId, SqlDbType.VarChar);
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(int?) || typeof(T) == typeof(Int16?))
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T) == typeof(DateTime?))
                aCmd.Parameters.Add(aId, SqlDbType.DateTime);
            else if (typeof(T) == typeof(float?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(Double?))
                aCmd.Parameters.Add(aId, SqlDbType.Float);
            else if (typeof(T) == typeof(bool?))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(bool))
                aCmd.Parameters.Add(aId, SqlDbType.Bit);
            else if (typeof(T) == typeof(Guid))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);
            else if (typeof(T) == typeof(Guid?))
                aCmd.Parameters.Add(aId, SqlDbType.UniqueIdentifier);


            else if (typeof(T).IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        typeof(T).GetGenericArguments()[0].IsEnum)
                aCmd.Parameters.Add(aId, SqlDbType.Int);
            else
            {
                throw new Exception("Type inconnu: " + typeof(T));
            }

            if (aValue == null)
                aCmd.Parameters[aId].Value = DBNull.Value;
            else
                aCmd.Parameters[aId].Value = aValue;
        }


        public static String GetNullableString(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? null : aReader.GetString(aNdx);
        }

        public static DateTime? GetNullableDate(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (DateTime?)null : aReader.GetDateTime(aNdx);
        }

        public static TimeSpan? GetNullableTimeSpam(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (TimeSpan?)null : aReader.GetTimeSpan(aNdx);
        }

        public static Guid GetNullableGuid(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? Guid.Empty : aReader.GetGuid(aNdx);
        }

        public static bool? GetNullableBool(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (bool?)null : aReader.GetBoolean(aNdx);
        }

        public static int? GetNullableInt(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (int?)null : aReader.GetInt32(aNdx);
        }

        public static Int16? GetNullableInt16(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (Int16?)null : aReader.GetInt16(aNdx);
        }

        public static double? GetNullableDouble(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (double?)null : aReader.GetDouble(aNdx);
        }
        public static float? GetNullableFloat(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (float?)null : aReader.GetFloat(aNdx);
        }
        public static Decimal? GetNullableDecimal(this SqlDataReader aReader, int aNdx)
        {
            return aReader.IsDBNull(aNdx) ? (Decimal?)null : aReader.GetDecimal(aNdx);
        }

    }
}
