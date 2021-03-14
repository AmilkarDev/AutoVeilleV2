using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Outils
{
    public class EnumHelpers
    {
        /// <summary>
        /// Retourne la description associe a un item d'un enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aValue"></param>
        /// <returns></returns>
        public static String GetValueDescription<T>( T aValue )
        {
            var type = typeof(T);
            var memInfo = type.GetMember(aValue.ToString());

            if (!memInfo.Any())
                return "";

            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes == null || !attributes.Any())
                return "";

            return ((DescriptionAttribute)attributes[0]).Description;
        }

    }
}
