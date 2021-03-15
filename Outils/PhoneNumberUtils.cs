using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Outils

{
    public class PhoneNumberUtils
    {
        public static String StripPhoneNumber(String aPhone)
        {
            if (String.IsNullOrEmpty(aPhone))
                return "";

            // Remove crap
            aPhone = new System.Text.RegularExpressions.Regex("\\s?").Replace(aPhone, "");
            aPhone = aPhone.Replace("-", "");
            aPhone = aPhone.Replace(")", "");
            aPhone = aPhone.Replace("(", "");

            return aPhone;
        }

        public static string NumberOnly(string aPhone)
        {
            if (String.IsNullOrEmpty(aPhone))
            {
                return "";
            }
            Regex digitsOnly = new Regex(@"[^\d]");
            aPhone = digitsOnly.Replace(aPhone, "");
            return aPhone;
        }

        public static String FormatPhoneNumber(String aPhone)
        {
            if (String.IsNullOrEmpty(aPhone))
                return "";

            aPhone = StripPhoneNumber(aPhone);

            if (aPhone.Length == 7)
            {
                return String.Format("{0}-{1}", aPhone.Substring(0, 3), aPhone.Substring(3));
            }

            if (aPhone.Length == 10)
            {
                return String.Format("({0}) {1}-{2}", aPhone.Substring(0, 3), aPhone.Substring(3, 3), aPhone.Substring(6));
            }

            return aPhone;
        }

        public static bool ValidePhoneNumber(String aPhone)
        {


            aPhone = StripPhoneNumber(aPhone);
            if (String.IsNullOrEmpty(aPhone))
                return false;
            char[] arr = aPhone.ToCharArray( );
            for (var i = 0; i < arr.Length; i++)
                if (!char.IsDigit(arr[i]))
                    return false;
            if ((aPhone.Length != 7 && aPhone.Length != 10 && aPhone.Length != 11) || (aPhone.Length == 11 && aPhone.Substring(0, 1) != "1"))
                return false;

            if (aPhone .Contains( "1111111" ) ||
                aPhone.Contains("2222222")  ||
                aPhone.Contains("3333333")  ||
                aPhone.Contains("4444444")  ||
                aPhone.Contains("5555555")  ||
                aPhone.Contains("6666666")  ||
                aPhone.Contains("7777777")  ||
                aPhone.Contains("8888888")  ||
                aPhone.Contains("9999999")  ||
                aPhone.Contains("0000000") 
                )
            {
                return false;
            }
                
            return true;
        }
    }
}
