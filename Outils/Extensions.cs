using System;
using System.Collections.Generic;

namespace Outils
{
    public static class Extensions
    {
        #region Strings extensions

        #region String to Numerical

        /// <summary>
        /// Checks if the string is numerical.
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>A value indicating if the string is numerical.</returns>
        public static bool IsNumeric(this string pString)
        {
            if (string.IsNullOrEmpty(pString)) return false;

            string pCultureInvariantString = pString.Replace(",", ".");

            float output;
            return float.TryParse(pCultureInvariantString, out output);
        }

        /// <summary>
        /// Checks if the string is an integer.
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>A value indicating if the string is an integer.</returns>
        public static bool IsInteger(this string pString)
        {
            int output;
            return int.TryParse(pString, out output);
        }

        /// <summary>
        /// Converts the string into an integer
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>The parsed value.</returns>
        public static int ToInteger(this string pString)
        {
            if (pString == null) throw new ArgumentNullException("pString");

            if(!string.IsNullOrEmpty(pString.Trim()))
            {
                if (pString.Trim().IsInteger())
                {
                    int output;
                    if (int.TryParse(pString.Trim(), out output))
                    {
                        return output;
                    }
                }
                
                throw new ArgumentException("Value is not an Integer.");
            }

            return 0;
        }

        #endregion

        /// <summary>
        /// Checks if the string contains jokers ('*', '%').
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>A value indicating if the string contains jokers.</returns>
        public static bool ContainsJokers(this string pString)
        {
            if (pString == null) throw new ArgumentNullException("pString");

            if(string.IsNullOrEmpty(pString.Trim())) return false;

            return pString.Contains("*") || pString.Contains("%");
        }

        /// <summary>
        /// Converts the jokers in the string to SQL jokers (['*', '%'] => ['%'])
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>The replaced string.</returns>
        public static string JokersToSQL(this string pString)
        {
            if (pString == null) throw new ArgumentNullException("pString");

            if (string.IsNullOrEmpty(pString.Trim())) return string.Empty;

            return pString.Replace('*', '%');
        }

        /// <summary>
        /// Format the string to a SQL string value.
        /// </summary>
        /// <param name="pString">The string.</param>
        /// <returns>The SQL formatted string value.</returns>
        public static string FormatStringToSQLString(this string pString)
        {
            if (pString == null) throw new ArgumentNullException("pString");

            if (string.IsNullOrEmpty(pString.Trim())) return null;

            const string tokenApostrophe = "&TOKENAPOSTROPHE&";
            var myNewString = pString;
            while (myNewString.StartsWith("'") || myNewString.EndsWith("'"))
            {
                myNewString = myNewString.Trim('\'');
            }

            return myNewString.Replace("''", tokenApostrophe).Replace("'", "''").Replace(tokenApostrophe, "''");
        }

        #endregion
        
        #region Linq Extensions

        /// <summary>
        /// Applies an action to each element in enumeration.
        /// </summary>
        /// <typeparam name="T">The type of element.</typeparam>
        /// <param name="pEnumeration">The enumeration.</param>
        /// <param name="pAction">The action to complete.</param>
        public static void ForEach<T>(this IEnumerable<T> pEnumeration, Action<T> pAction)
        {
            if (pEnumeration == null) throw new ArgumentNullException("pEnumeration");
            if (pAction == null) throw new ArgumentNullException("pAction");

            foreach (T item in pEnumeration)
            {
                pAction(item);
            }
        }

        /// <summary>
        /// Applies an action to each element in enumeration. Passes the enumeration index to the action.
        /// </summary>
        /// <typeparam name="T">The type of element.</typeparam>
        /// <param name="pEnumeration">The enumeration.</param>
        /// <param name="pAction">The action to complete.</param>
        public static void ForEach<T>(this IEnumerable<T> pEnumeration, Action<T, int> pAction)
        {
            if (pEnumeration == null) throw new ArgumentNullException("pEnumeration");
            if (pAction == null) throw new ArgumentNullException("pAction");

            int i = 0;
            foreach (T item in pEnumeration)
            {
                pAction(item, i);
                i++;
            }
        }

        #endregion

        #region ExceptionsExtensions

        /// <summary>
        /// Format the exception to a comprehensible string.
        /// </summary>
        /// <param name="pException">The exception.</param>
        /// <returns>The string.</returns>
        public static string GetExceptionString(this Exception pException)
        {
            if (pException == null) throw new ArgumentNullException("pException");

            return Utils.CreateExceptionString(pException);
        }

        #endregion
    }
}
