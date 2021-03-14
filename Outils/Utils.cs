//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Outils
{
    /// <summary>
    /// Utils for the Suly core.
    /// </summary>
    public static class Utils
    {
        #region Methods

        #region Public Methods

        	#region Class Conversion
		/// <summary>
		/// Converts an object of a class into another, creating the destination object and moving the content of similar fields from the source object.
		/// The copy is a shallow copy.  There is no distinction between properties and fields: a proprty may be copied in a field of the same type
		/// and vice-versa.  The datatype must match excactly, including the nullability. Only instance fields and properties are considered.
		/// </summary>
		/// <typeparam name="T">Type of the destination class.</typeparam>
		/// <param name="pSource">The source object.</param>
		/// <returns>A new instance of the destination object.</returns>
		public static T ConvertToClass<T>(Object pSource) where T : class, new()
		{
			return ConvertToClass<T>(pSource, null);
		}

		/// <summary>
		/// Converts an object of a class into another, creating the destination object and moving the content of similar fields from the source object.
		/// The copy is a shallow copy.  There is no distinction between properties and fields: a proprty may be copied in a field of the same type
		/// and vice-versa.  The datatype must match excactly, including the nullability. Only instance fields and properties are considered.
		/// </summary>
		/// <typeparam name="T">Type of the destination class.</typeparam>
		/// <param name="pSource">The source object.</param>
		/// <param name="pOptions">
		/// A series of field equivalences.  Allows you to esatblish crrespondances between fields with different names or to leave out sone source fields.
		/// The parameter is either a dictionary or an anonymous objevts whose properties are source names and values are strings with the destination name.
		/// A null parameter value will exclude the field from the copy.
		/// </param>
		/// <returns>A new instance of the destination object.</returns>
		public static T ConvertToClass<T>(Object pSource, Object pOptions) where T : class, new()
		{
			T destination = new T();
			ConvertToClass(pSource, destination, pOptions);
			return destination;
		}

		/// <summary>
		/// Converts an object of a class into another, creating the destination object and moving the content of similar fields from the source object.
		/// The copy is a shallow copy.  There is no distinction between properties and fields: a proprty may be copied in a field of the same type
		/// and vice-versa.  The datatype must match excactly, including the nullability. Only instance fields and properties are considered.
		/// </summary>
		/// <param name="pSource">The source object.</param>
		/// <param name="pDestination">A destination object into which values will be copied.</param>
		public static void ConvertToClass(Object pSource, Object pDestination)
		{
			ConvertToClass(pSource, pDestination, null);
		}

		/// <summary>
		/// Converts an object of a class into another, creating the destination object and moving the content of similar fields from the source object.
		/// The copy is a shallow copy.  There is no distinction between properties and fields: a proprty may be copied in a field of the same type
		/// and vice-versa.  The datatype must match excactly, including the nullability. Only instance fields and properties are considered.
		/// </summary>
		/// <param name="pSource">The source object.</param>
		/// <param name="pDestination">A destination object into which values will be copied.</param>
		/// <param name="pOptions">
		/// A series of field equivalences.  Allows you to esatblish crrespondances between fields with different names or to leave out sone source fields.
		/// The parameter is either a dictionary or an anonymous objevts whose properties are source names and values are strings with the destination name.
		/// A null parameter value will exclude the field from the copy.
		/// </param>
		public static void ConvertToClass(Object pSource, Object pDestination, Object pOptions)
		{
			var options = Utils.ToDictionary(pOptions);

			//--- Build a dicitonary of properties, keyed by the name they will have in the destination.

			var convertEntries = new Dictionary<String, ConvertEntry>();
			var sourceType = pSource.GetType();
			foreach (var propInfo in sourceType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				var option = options.GetValue(propInfo.Name, "?");
				if (option != null && option != "*")
				{
					var destinationName = option == "?" ? propInfo.Name : option;
					convertEntries.Add(destinationName, new ConvertEntry { DestinationName = destinationName, MemberInfo = propInfo });
				}
			}
			foreach (FieldInfo fieldInfo in sourceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
				if (!fieldInfo.Name.EndsWith("_BackingField"))
				{
					var option = options.GetValue(fieldInfo.Name, "?");
					if (option != null && option != "*")
					{
						String destinationName = option == "?" ? fieldInfo.Name : option;
						convertEntries.Add(destinationName, new ConvertEntry { DestinationName = destinationName, MemberInfo = fieldInfo });
					}
				}

			//--- Loop on the destination properties and fields and establish a correspondance.

			Type destType = pDestination.GetType();
			foreach (PropertyInfo propInfo in destType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				ConvertEntry convertEntry;
				if (convertEntries.TryGetValue(propInfo.Name, out convertEntry))
				{
					Type origType;
					Object origValue;
					if (convertEntry.MemberInfo is PropertyInfo)
					{
						origType = ((PropertyInfo)convertEntry.MemberInfo).PropertyType;
						origValue = ((PropertyInfo)convertEntry.MemberInfo).GetValue(pSource, null);
					}
					else
					{
						origType = ((FieldInfo)convertEntry.MemberInfo).FieldType;
						origValue = ((FieldInfo)convertEntry.MemberInfo).GetValue(pSource);
					}
					if (origType == propInfo.PropertyType)
						propInfo.SetValue(pDestination, origValue, null);
				}
			}
			foreach (FieldInfo fieldInfo in destType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				ConvertEntry convertEntry;
				if (convertEntries.TryGetValue(fieldInfo.Name, out convertEntry))
				{
					Type origType;
					Object origValue;
					if (convertEntry.MemberInfo is PropertyInfo)
					{
						origType = ((PropertyInfo)convertEntry.MemberInfo).PropertyType;
						origValue = ((PropertyInfo)convertEntry.MemberInfo).GetValue(pSource, null);
					}
					else
					{
						origType = ((FieldInfo)convertEntry.MemberInfo).FieldType;
						origValue = ((FieldInfo)convertEntry.MemberInfo).GetValue(pSource);
					}
					if (origType == fieldInfo.FieldType)
						fieldInfo.SetValue(pDestination, origValue);
				}
			}
		}

		/// <summary>
		/// This is a structure used to keep track of properties specifications.
		/// </summary>
		private class ConvertEntry
		{
			/// <summary>
			/// The name it will have in the destination class.  If not provided, the same as the source name.
			/// </summary>
			public String DestinationName;

			/// <summary>
			/// The reflection info used to copy the data.
			/// </summary>
			public MemberInfo MemberInfo;
		}
		#endregion

		/// <summary>
		/// Returns an ID uniquely identifying the build for this assembly.
		/// </summary>
		/// <param name="pAssembly"></param>
		/// <returns></returns>
		/// <remarks>
		/// We use the Revision number.
		/// </remarks>
		public static String GetUniqueBuildId(Assembly pAssembly)
		{
			int revisionNumber = pAssembly.GetName().Version.Revision;
			if (revisionNumber < 100)
				throw new Exception("Cannot devise an assembly unique name.");
			return String.Format("Id{0}", revisionNumber);
		}

        /// <summary>
        /// Get's the user's local setting from an http request and set's it on the current thread.
        /// </summary>
        public static void SetUserLocaleFromHttpRequest()
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();

                try
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Lang);
                }
                catch
                { ;}
            }
        }



        /// <summary>
        /// Retruns the application name.  In a web application, this is the first level of the URL.
        /// </summary>
        public static String ApplicationName
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;
                if (httpContext != null)
                    return httpContext.Request.Path.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
                else
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    if (assembly != null)
                        return assembly.GetName().Name;

                    var currentProcess = Process.GetCurrentProcess();
                    return currentProcess.ProcessName;
                }
            }
        }

        /// <summary>
        /// Returns the path to the main executable of the application.  It works with web applications too.
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                string basePath;
                if (HttpContext.Current != null)
                    basePath = HttpContext.Current.Request.PhysicalApplicationPath;
                else if (OperationContext.Current != null)
                    basePath = HostingEnvironment.ApplicationPhysicalPath;
                else
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly() ?? typeof(Utils).Assembly;
                    basePath = Path.GetDirectoryName(entryAssembly.Location);
                }
                if (basePath != null && !basePath.EndsWith("\\"))
                    basePath += "\\";
                return basePath;
            }
        }

        /// <summary>
        /// Creates a comprehensible string from the exception.
        /// </summary>
        /// <param name="pException">The exception.</param>
        /// <returns>The comprehensible string.</returns>
        public static string CreateExceptionString(Exception pException)
        {
            StringBuilder stringBuilder = new StringBuilder();
            CreateExceptionString(stringBuilder, pException, 0);

            return stringBuilder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a comprehensible string from the exception.
        /// </summary>
        /// <param name="pStringBuilder">Th string builder used to create the string.</param>
        /// <param name="pException">The exception.</param>
        /// <param name="pIndent">The number of times a space should be used as indent.</param>
        private static void CreateExceptionString(StringBuilder pStringBuilder, Exception pException, int pIndent)
        {
            string indent = new string(' ', pIndent);

            if (pIndent > 0)
            {
                pStringBuilder.AppendLine(string.Format("{0}Inner Exception:", indent));
            }
            else
            {
                pStringBuilder.AppendLine("Exception Found:");
            }

            pStringBuilder.AppendLine(string.Format("{0}Type: {1}", indent, pException.GetType().FullName));
            pStringBuilder.AppendLine(string.Format("{0}Message: {1}", indent, pException.Message));
            pStringBuilder.AppendLine(string.Format("{0}Source: {1}", indent, pException.Source));
            pStringBuilder.AppendLine(string.Format("{0}Stacktrace: {1}", indent, pException.StackTrace));

            if (pException.InnerException != null)
            {
                pStringBuilder.AppendLine("");
                CreateExceptionString(pStringBuilder, pException.InnerException, pIndent + 2);
            }
        }

        public static DateTime? ConvertDate(String aCulture, String aDate)
        {
            DateTime? res = null;

            try
            {
                IFormatProvider culture = new System.Globalization.CultureInfo(aCulture, true);

                // Alternate choice: If the string has been input by an end user, you might 
                // want to format it according to the current culture:
                // IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (!String.IsNullOrEmpty(aCulture) && !String.IsNullOrEmpty(aDate))
                {

                    res = (DateTime.Parse(aDate, culture, System.Globalization.DateTimeStyles.AssumeLocal));
                }

            }
            catch
            {
                res = null;
            }
            return res;
        }

        public static DateTime? ConvertDate(String aCulture, String aDate, String[] aFormats)
        {
            DateTime? res = null;

            try
            {
              
                CultureInfo culture = new CultureInfo(aCulture, true);
                // Alternate choice: If the string has been input by an end user, you might 
                // want to format it according to the current culture:
                // IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (!String.IsNullOrEmpty(aCulture) && !String.IsNullOrEmpty(aDate))
                {

                    res = DateTime.ParseExact(aDate, aFormats, culture, DateTimeStyles.None); 
                }

            }
            catch (Exception ex)
            {
                res = null;
            }
            return res;
        }
        public static String GetDateText(String aLangue, DateTime aDate)
        {
            var resultat = aDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("fr-Fr"));
            if (aLangue == "A")
            {
                resultat = aDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            }
            return resultat;
        }

        public static String GetDateText(String aLangue, DateTime aDate, String aFormat)
        {
            var resultat = aDate.ToString(aFormat, CultureInfo.CreateSpecificCulture("fr-Fr"));
            if (aLangue == "A")
            {
                resultat = aDate.ToString(aFormat, CultureInfo.CreateSpecificCulture("en-US"));
            }
            return resultat;
        }

        #endregion
        #region Anomymous Object to Dictionary Conversion
        /// <summary>
        /// This method converts the properties of an anonymous object to a dictionary of values.
        /// Properties are set as the keys and values as the value.
        /// </summary>
        /// <param name="pObject"></param>
        public static Dictionary<String, Object> ToDictionary(Object pObject)
        {
            if (pObject == null)
                return new Dictionary<String, Object>();

            //--- If the input received is already in the desired format simply return it.

            if (pObject is Dictionary<String, Object>)
                return pObject as Dictionary<String, Object>;

            //--- Otherwise, loop on all fields and properties.

            var props = pObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var results = props.ToDictionary(prop => prop.Name, prop => prop.GetValue(pObject, null));

            return results;

        }
        		/// <summary>
		/// A practical way of fetching a value from a dictionary of the previous type.
		/// </summary>
		/// <typeparam name="T">The type of the desired data value.</typeparam>
		/// <param name="pDict">The dictionary from which to fetch the value.</param>
		/// <param name="pKey">The key to the value to fetch.</param>
		/// <param name="pDefault">The value to be returned if the option is not found.</param>
		/// <returns></returns>
		public static T GetValue<T>(this Dictionary<String, Object> pDict, String pKey, T pDefault)
		{
			Object result;
			if (!pDict.TryGetValue(pKey, out result))
				return pDefault;
			return (T)result;
		}

        public static String GetValueAsString(this Dictionary<String, Object> pDict, String pKey, String pDefault)
        {
            Object result;
            if (!pDict.TryGetValue(pKey, out result) || result == null)
                return pDefault ?? "";
            return result.ToString();
        }




		#endregion

        #endregion

        public static string EmptyIfNull(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        public static bool ValideEmail(String aEmail)
        {
            if (String.IsNullOrEmpty(aEmail))
            {
                return false;
            }
            var index = aEmail.IndexOf("@");
            if (index == -1)
            {
                return false;
            }
            else
            {
                index = (aEmail.Substring(index, aEmail.Length-index)).IndexOf(".");
                if (index == -1)
                {
                    return false;
                }
                if (aEmail.Substring(aEmail.Length-1, 1) == ".")
                {
                    return false;
                }
            }

            return true;
        }

        public static bool VerifyFileInFolder(string aFile, string aRoot)
        {
            if (String.IsNullOrEmpty(aRoot)) return false;
            if (String.IsNullOrEmpty(aFile)) return false;
            if (!Directory.Exists(aRoot)) return false;

            DirectoryInfo di = new DirectoryInfo(aRoot);
            FileInfo[] TXTFiles = di.GetFiles(aFile);
            if (TXTFiles.Length == 0)
            {
                return false;
            }
            return true;
        }

        public static string GetMonthName(int aMonth, string aLangue)
        {
            return new DateTime(1900, aMonth, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture(aLangue));
        }

        public static int GetMonthNumero(string aMonth, string aLangue)
        {
            string[] fomrat = new string[1];
            fomrat[0] = "dd MMMM yyyy";
            var dateRes = ConvertDate("fr-CA", "01 "+aMonth+" 1900",fomrat);
            var res = 0;
            var dateResNotNull=new DateTime();
            if (dateRes != null)
            {
                dateResNotNull = (DateTime) dateRes;
                res = dateResNotNull.Month;
            }
            
            return res;
        }

        public static string GetBuild()
        {
            #if RELEASE
                return "";
            #else
            #if QA 
                return " Build QA";
            #else
                        return " Build Debug";
            #endif
            #endif
        }
        //#if RELEASE   
        //            LbVersion.Content =LbVersion.Content  + " " + "Build: QA";

        //#else
        //            LbVersion.Content = LbVersion.Content + " " + "Build: autres que production";
        //#endif

        public static void ExportGenericListToExcel<T>(List<T> list, string excelFilePath, string excelNameFile)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //Application ExcelApp = new Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //ExcelApp.Application.Workbooks.Add(excelFilePath + excelNameFile);

            SLDocument sl = new SLDocument();
            SLStyle style = sl.CreateStyle();
            style.FormatCode = "dd-mm-yyyy";
            
            for (int i = 0; i < properties.Count; i++)
                sl.SetCellValue(1, i + 1, properties[i].Name);

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < properties.Count; j++)
                {
                    string typeName = "String";
                    if (properties[j].GetValue(list[i]) != null)
                    {
                        var type = properties[j].GetValue(list[i]).GetType();
                        typeName = type.Name;
                    }
                   
                    //if (properties[j].Name == "DateAchat")
                    if (typeName == "DateTime")
                    {
                        if (properties[j].GetValue(list[i]) == null)
                        {
                            sl.SetCellValue(i + 2, j + 1, "");
                        }
                        else
                        {
                            sl.SetCellValue(i + 2, j + 1, (DateTime)properties[j].GetValue(list[i]));
                            sl.SetCellStyle(i + 2, j + 1, style);
                        }
                        
                        //sl.SetCellValue(i + 2, j + 1, properties[j].GetValue(list[i]) == null ? "" : properties[j].GetValue(list[i]).ToString());                        
                    }
                    else
                    {
                        sl.SetCellValue(i + 2, j + 1, properties[j].GetValue(list[i]) == null ? "" : properties[j].GetValue(list[i]).ToString());                       
                    }
                }
            }
      
            //Utils.ExportGenericListToExcel(vp, @"d:\temp\TestVP.xlsx");
            sl.SaveAs(excelFilePath +  excelNameFile);

        }


        public static string DetermineCompName(string IP)
        {
            //IPAddress myIP = IPAddress.Parse(IP);
            //IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            //List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            //return compName.First();
            return "";
        }

        //public static void ExportGenericListToExcel<T>(List<T> list, string excelFilePath, string excelNameFile)
        //{
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //    Application ExcelApp = new Application();
        //    ExcelApp.Application.Workbooks.Add(Type.Missing);
        //    ExcelApp.Application.Workbooks.Add(excelFilePath + excelNameFile);

          

        //    for (int i = 0; i < properties.Count; i++)
        //        ExcelApp.Cells[1, i+1].Value = properties[i].Name;

        //    for (int i = 0; i < list.Count; i++)
        //        for (int j = 0; j < properties.Count; j++)
        //            ExcelApp.Cells[i + 2, j+1].Value = properties[j].GetValue(list[i]);

        //    ExcelApp.ActiveWorkbook.SaveCopyAs(excelFilePath +"117" + excelNameFile);

        //    ExcelApp.ActiveWorkbook.Saved = true;
        //    ExcelApp.Quit();

         
        //}

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public static double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public static double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public static string suppChar(string aText)
        {
            if (aText == null)
            {
                aText = "";
            }

            var res = aText;
            res = res.Replace("è", "e");
        
             res = res.Replace( "é", "e");
            res = res.Replace("ç", "c");
             res = res.Replace( "à", "a");
             res = res.Replace( "ù", "u");
             res = res.Replace( "ô", "o");
             res = res.Replace( "ê", "e");  
             res = res.Replace( "î", "i") ;
             res = res.Replace( "è", "e") ;
             res = res.Replace( "-", "") ;
             //res = res.Replace( """""", "");
             res = res.Replace( " ", "") ;
             res = res.Replace( "(", "") ;
             res = res.Replace( ")", "") ;
             res = res.Replace( "/", "") ;
             res = res.Replace( "&", "") ;
             res = res.Replace( @"\", "") ;
             res = res.Replace( ".", "") ;
             //res = res.Replace( """, "") ;
             res = res.Replace( ",", "") ;
            res = res.Replace("*", "");
            res = res.Replace("#", "");
            res = res.Replace("_", "");
            res = res.Replace(";", "");
             res = res.Replace( "`", "") ;
             res = res.Replace( "ï", "i") ;	
             res = res.Replace( "ë", "e") ;	
             res = res.Replace( "ö", "o") ;
             res = res.Replace( "ä", "a") ;
             res = res.Replace( "?", "") ;
            res = res.Replace("$", "");
            res = res.Replace("@", "");
            res = res.Replace("!", "");
            res = res.Replace("|", "");
            res = res.Replace("!", "");
            res = res.Replace("{", "");
            res = res.Replace("}", "");
            res = res.Replace("[", "");
            res = res.Replace("]", "");
            res = res.Replace("~", "");
             res = res.Replace( "%", "") ;
             res = res.Replace( "â", "a") ;
            res = res.Replace("œ", "oe");
            return res;
        }


        public static string MakeFirstUpperMarque(string aMarque)
        {
            var res = aMarque;
            if (aMarque == "GMC")
            {
                return res;
            }
                char[] resArray = res.ToCharArray();
                resArray[0] = char.ToUpper(resArray[0]);
                var i = 0;
                for( i=1;i<resArray.Length-1;i++)
                {
                    if (resArray[i] == ' ' || resArray[i] == '-')
                    {
                        resArray[i + 1] = char.ToUpper(resArray[i+1]);                   
                    }
                    else
                    {
                        resArray[i] = char.ToLower(resArray[i]);  
                    }
                }
                resArray[i] = char.ToLower(resArray[i]);  
                res= new string(resArray);
            return res;
        }

        public static bool ValidateCodePostal(string aCodePostal)
        {
            var matches = Regex.Match(aCodePostal.ToUpper().Replace(" ",""), "([A-Z][0-9][A-Z][0-9][A-Z][0-9])");
            if (matches.Success)
            {
                return true;
            }
            else
            {
               return false;
            }
        }


        public static bool ValidateProvinceCanada(string aProvince)
        {

            switch (aProvince.ToUpper())
            {
                case "AB":
                    return true;
                case "BC":
                    return true;
                case "PC":
                    return true;
                case "MB":
                    return true;
                case "NB":
                    return true;
                case "NS":
                    return true;
                case "ON":
                    return true;
                case "QC":
                    return true;
                case "SK":
                    return true;
                case "NL":
                    return true;
                case "NU":
                    return true;
                case "NT":
                    return true;
                case "YT":
                    return true;
                default:                
                    return false;
                
            }
        }

        /// <summary>
        /// Compare 2 string en retirant toutes leurs valeurs non alphanumériques.
        /// Les 2 string formattés doivent être identique pour retourner true.
        /// * Les valeurs Null sont considéré indéfini et retourneront toujours false.
        /// </summary>
        /// <param name="source">string sur laquelle est appliquée la fonction</param>
        /// <param name="toCompare">string à comparer</param>
        /// <param name="caseSensitive">indique si la comparaison doit tenir compte des majuscules/minuscules (optionnel : false)</param>
        /// <returns></returns>
        public static bool CompareAlphaNum(this string source, string toCompare, bool caseSensitive = false)
        {
            if (source == null || toCompare == null)
                return false;

            var formattedSource = Regex.Replace(source, "[^A-Za-z0-9]", "");
            var formattedToCompare = Regex.Replace(toCompare, "[^A-Za-z0-9]", "");

            if (!caseSensitive)
            {
                formattedSource = formattedSource.ToLower();
                formattedToCompare = formattedToCompare.ToLower();
            }

            return formattedSource.Equals(formattedToCompare);
        }
        /// <summary>
        /// Génère un password aléatoire contenant des caractères alphanumérique en quantité égale
        /// contenant un nombre de caractères défini par l'utilisateur.
        /// *Le nombre de caractères doit être compris entre 4 et 100
        /// </summary>
        /// <param name="length">nombre de caractères à générer</param>
        /// <returns>Mot de passe généré</returns>
        public static string GenerateAlphaNumPassword(int length)
        {
            if (length < 4 || length > 100)
                throw new ArgumentOutOfRangeException("La valeur doit être comprise entre 4 et 100");
            string res = "";

            Random random = new Random();

            List<char> charList = new List<char>();

            for (int i = 0; i < length; i++)
            {
                var isNum = i % 2 == 1;
                charList.Add((char)(isNum ? (random.Next(48, 57)) : (random.Next(0, 100) < 50 ? (random.Next(97, 122)) : (random.Next(65, 90)))));
            }

            List<int> position = Enumerable.Range(0, length).ToList();

            while (charList.Count > 0)
            {
                var i = position[(random.Next(0, charList.Count - 1))];
                res += charList[i];
                charList.RemoveAt(i);
            }

            return res;
        }
        /// <summary>
        /// Format une adresse selon les standard de formattage des adresses Suly
        /// </summary>
        /// <param name="address">L'adresse à formatter</param>
        /// <returns>Adresse formatté</returns>
        public static string FormatAddress(string address)
        {
            List<string> typesRue = new List<string>() { "avenue", "boul.", "rue", "croissant", "terrasse", "place", "C.P.", "rang", 
                                                        "route", "chemin", "app.", "carré", "bureau","impasse", "montée","succ", "allée", "RR", "unité" };

            address = address.Trim();

            // Retirer les charactères indésirables 
            Regex badCharRgx = new Regex(@"[:;,/\\ ]+");
            address = badCharRgx.Replace(address, " ");

            // ajuster la case en 'camel case'
            address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(address.ToLower());

            //Mettre les déterminants en minuscules
            Regex determinantLaRgx = new Regex(" (la) ", RegexOptions.IgnoreCase);
            address = determinantLaRgx.Replace(address, " la ");

            Regex determinantDRgx = new Regex(" (d)((es?)|u) ", RegexOptions.IgnoreCase);
            address = determinantDRgx.Replace(address, " d$2 ");

            // Ajouter espace entre lettre et nombre
            Regex addSpaceRgx = new Regex(@"(\d+)([a-z]+)", RegexOptions.IgnoreCase);
            address = addSpaceRgx.Replace(address, "$1 $2");

            // Corriger les St/Ste
            Regex stRgx = new Regex(@" s(ain)?t(e?)-? ?", RegexOptions.IgnoreCase);
            address = stRgx.Replace(address, " St$2-");

            // Corriger les xe
            Regex eRgx = new Regex(@"([\d]+)( *)((i?[eè]?me?s?)|e) ", RegexOptions.IgnoreCase);
            address = eRgx.Replace(address, "$1e ");

            // Corriger les xer
            Regex erRgx = new Regex(@"(\d) ?(er) ", RegexOptions.IgnoreCase);
            address = erRgx.Replace(address, "$1er ");

            // Corriger les xère
            Regex ereRgx = new Regex(@"(\d) ?(i?[eè]?re )", RegexOptions.IgnoreCase);
            address = ereRgx.Replace(address, "$1ère ");

            // Corriger l'élidation
            Regex elidationRgx = new Regex(@"( [ld])[' ]([aeiou])", RegexOptions.IgnoreCase);
            var elidationMatch = elidationRgx.Match(address);
            if (elidationMatch.Success)
                address = elidationRgx.Replace(address, String.Format("{0}'{1}", elidationMatch.Groups[1].Value.ToUpper(), elidationMatch.Groups[2].Value.ToUpper()));

            // Corriger les app.
            Regex appRgx = new Regex(@"app?t?\.?(art(ement)?)? *#?(\d+| [a-z])", RegexOptions.IgnoreCase);
            address = appRgx.Replace(address, " app. $3");

            // Corriger les rang
            Regex rangRgx = new Regex(@"([\d ]{1}rn?g\.? )|([\d ]{1}rn?g\.?$)", RegexOptions.IgnoreCase);
            address = rangRgx.Replace(address, " rang ");

            // Corriger les bureau
            Regex bureauRgx = new Regex(@" ((bur(eau)?)|(suite))\.? ", RegexOptions.IgnoreCase);
            address = bureauRgx.Replace(address, " bureau ");

            // corriger les déterminants de lieu autre que app.
            address = address.Replace(" pl ", " place ");
            address = address.Replace(" imp ", " impasse ");

            // corriger les  mgr
            Regex mgrRgx = new Regex(@" mgr[ \.-]", RegexOptions.IgnoreCase);
            address = mgrRgx.Replace(address, " Monseigneur-");

            // Corriger les routes
            Regex routeRgx = new Regex(@" ((r(ou)?te?)|(ro?a?d))[\. ]", RegexOptions.IgnoreCase);
            address = routeRgx.Replace(address, " route ");

            // Retirer les # indésirable (si déterminant de lieu existe) ou le remplacer par app.
            Regex numRgx = new Regex(@" ap(([p]\.?)|t)| suite| bureau| route| rang", RegexOptions.IgnoreCase);
            if (address.Contains('#'))
                if (numRgx.IsMatch(address))
                    address = address.Replace("#", " ");
                else
                    address = address.Replace("#", " app. ");

            // Corrger les Case postale
            Regex cpRgx = new Regex(@" [c][ ]?[.]?[ ]?[p][.]?", RegexOptions.IgnoreCase);
            address = cpRgx.Replace(address, " C.P. ");

            //ajouter un espace entre les numero de rue et les types de rue
            Regex strRgx = new Regex(@"(\d+)(rue|rang|route|av|ch|boul|bl|bd|des )", RegexOptions.IgnoreCase);
            address = strRgx.Replace(address, "$1 $2");

            //Corriger les 'avenue'
            Regex aveRgx = new Regex(@" av(e(nue)?)?\.? ", RegexOptions.IgnoreCase);
            address = aveRgx.Replace(address, " avenue ");

            //Corriger les 'boul'
            Regex boulRgx = new Regex(@" (boul|blv?d?)\.? ", RegexOptions.IgnoreCase);
            address = boulRgx.Replace(address, " boul. ");

            //Corriger les 'chemin'
            Regex cheminRgx = new Regex(@"([\d ])(ch(emin|\.)?) ", RegexOptions.IgnoreCase);
            address = cheminRgx.Replace(address, "$1 chemin ");

            //Corriger les 'Route rurales'
            Regex rrRgx = new Regex(@" r\.?r\.? ?(\d+)", RegexOptions.IgnoreCase);
            address = rrRgx.Replace(address, " RR $1 ");

            //Corriger les 'Places'
            Regex placeRgx = new Regex(@" pl(ace)?[\. ]", RegexOptions.IgnoreCase);
            address = placeRgx.Replace(address, " place ");

            //Corriger les 'unité'
            Regex uniteRgx = new Regex(@" unit[eé] ", RegexOptions.IgnoreCase);
            address = uniteRgx.Replace(address, " unité ");

            //Corriger les 'allée'
            Regex alleeRgx = new Regex(@" all[eé]e? ", RegexOptions.IgnoreCase);
            address = alleeRgx.Replace(address, " allée ");

            //Corriger les 'montée'
            Regex monteeRgx = new Regex(@" mont[eé]e? ", RegexOptions.IgnoreCase);
            address = monteeRgx.Replace(address, " montée ");

            //Corriger les 'croissant'
            Regex croissantRgx = new Regex(@" cr(ois(sant)?)?[ \.]", RegexOptions.IgnoreCase);
            address = croissantRgx.Replace(address, " croissant ");

            //remplacer les app lettres suivant le numéro de maison par app. x
            Regex appLettreRgx = new Regex(@"^(\d*) *([a-z]) ", RegexOptions.IgnoreCase);
            var appLettreMatch = appLettreRgx.Match(address);
            if (appLettreMatch.Success)
                address = appLettreRgx.Replace(address, String.Format("{0} app. {1}", appLettreMatch.Groups[1].Value, appLettreMatch.Groups[2].Value.ToUpper()));

            //remplacer les #app devant tiret par app. #
            Regex appTiretRgx = new Regex(@"^([a-z]?\d+) *- *(\d+)", RegexOptions.IgnoreCase);
            address = appTiretRgx.Replace(address, "$2 app. $1");

            // Corriger certains types de rues
            address = address.Replace(" Rue ", " rue ");

            //Mettre les types de rue suivant les numérique en majuscule (ex: 3e Avenue, 57e rue)
            Regex numTypeRueRgx = new Regex(@"(\d+e )([a-z])([a-z]+)", RegexOptions.IgnoreCase);
            var numTypeRueMatch = numTypeRueRgx.Match(address);
            if (numTypeRueMatch.Success)
                address = numTypeRueRgx.Replace(address, String.Format("{0}{1}{2}", numTypeRueMatch.Groups[1].Value, numTypeRueMatch.Groups[2].Value.ToUpper(), numTypeRueMatch.Groups[3].Value));

            // Corriger les points cardinaux (attention au Est pour ne pas  confondre les numérique (2e ave)
            Regex NordRgx = new Regex(@"(?<!app.)([ -])(n |n$)", RegexOptions.IgnoreCase);
            address = NordRgx.Replace(address, " Nord ");

            Regex SudRgx = new Regex(@"(?<!app.)([ -])(s |s$)", RegexOptions.IgnoreCase);
            address = SudRgx.Replace(address, " Sud ");

            Regex OuestRgx = new Regex(@"(?<!app.)([ -])(o |o$)", RegexOptions.IgnoreCase);
            address = OuestRgx.Replace(address, " Ouest ");

            Regex EstRgx = new Regex(@"(?<!app.)([ -])(e |e$)", RegexOptions.IgnoreCase);
            address = EstRgx.Replace(address, " Est ");

            // Ajouter 'app.' devant les nombre seul 
            Regex nbSeulRgx = new Regex(@"([a-z0-9\.-]*) +(\d+)$", RegexOptions.IgnoreCase);
            var nbSeulMatch = nbSeulRgx.Match(address);
            if (nbSeulMatch.Success && typesRue.All(x => x.ToLower() != nbSeulMatch.Groups[1].Value.ToLower()))
                address = nbSeulRgx.Replace(address, String.Format("{0} app. {1}", nbSeulMatch.Groups[1].Value, nbSeulMatch.Groups[2].Value.ToUpper()));

            // Mettre des - entre les nom commun
            string copy = address.Replace('-', ' ');

            // Enlever les app. x de la copie
            Regex removeappRgx = new Regex(@"(app\. +)(\d+|[a-z])", RegexOptions.IgnoreCase);
            copy = removeappRgx.Replace(copy, " ");

            // Enlever déterminants
            Regex removeDetRgx = new Regex(" les | le | des | de (la )?| la | du ", RegexOptions.IgnoreCase);
            copy = removeDetRgx.Replace(copy, " ");

            // Enlever types Rues
            Regex removestrTypesRgx = new Regex(" " + string.Join(" | ", typesRue) + " ", RegexOptions.IgnoreCase);
            copy = removestrTypesRgx.Replace(copy, " ");

            // Enlever points Cardinaux
            Regex removecardPtRgx = new Regex(" (Nord|Sud|Ouest|Est) ?", RegexOptions.IgnoreCase);
            copy = removecardPtRgx.Replace(copy, " ");

            // Mettre les app. x à la fin
            Regex appxRgx = new Regex(@"(app\. +)([a-z]?\d*)", RegexOptions.IgnoreCase);
            Match appxMatch = appxRgx.Match(address);
            if (appxMatch.Success)
                address = appxRgx.Replace(address, " ") + " " + appxMatch.Value;

            // Remplacer tout les espaces multiples par un seul espace
            Regex spaceRgx = new Regex(@"[ ]+");
            address = spaceRgx.Replace(address, " ");

            Regex numberRgx = new Regex(@"(\d+)((e|(ère)|[a-z]) )?", RegexOptions.IgnoreCase);
            copy = numberRgx.Replace(copy, "");
            copy = copy.Trim();

            if (copy.Length > 0)
                address = address.Replace('-', ' ').Replace(copy, copy.Replace(' ', '-'));

            // Ajouter '-' entre 'nombre et nombre'
            Regex nombreEtNombreRgx = new Regex(@"(\d) *et *(\d)", RegexOptions.IgnoreCase);
            address = nombreEtNombreRgx.Replace(address, "$1-Et-$2");

            // Ajouter les accents é au début
            Regex AccentAiguDebutRgx = new Regex(@"( E)([^asr 0-9][a-z])", RegexOptions.IgnoreCase);
            address = AccentAiguDebutRgx.Replace(address, " É$2");

            // Ajouter les accents é à la fin 
            Regex AccentAiguFinRgx = new Regex(@" ([a-z]+)(e)(es? )", RegexOptions.IgnoreCase);
            address = AccentAiguFinRgx.Replace(address, "$1é$3");


            return address;
        }

        public static string Mins15ToString(int aMinsInit)
        {
            var mins = aMinsInit / 15;
            var mins15 = 15 * mins;
            var heure = mins15 / 60;
            var heureText = heure < 10 ? "0" + heure.ToString() : heure.ToString();
            var minsRest = mins15 - heure * 60;
            var minsText = minsRest < 10 ? "0" + minsRest.ToString() : minsRest.ToString();
            return heureText + "h" + minsText;
        }

        public static string Mins15TexteFormat(string aMinsInit)
        {

            if (aMinsInit == null || aMinsInit == "" || aMinsInit.Length < 3)
            {
                return aMinsInit;
            }
            if (aMinsInit != null)
            {
                aMinsInit=aMinsInit.Trim();
                aMinsInit=aMinsInit.Replace(" ", "");
                aMinsInit = aMinsInit.Replace(":", "h");
                aMinsInit = aMinsInit.Replace("H", "h");
                if (aMinsInit.Substring(1, 1) == "h")
                {
                    aMinsInit = "0" + aMinsInit;
                }
                if (aMinsInit.Length == 4 && aMinsInit.Substring(2, 2) == "h0")
                {
                    aMinsInit = aMinsInit + "0";
                }
            }
            return aMinsInit;
        }

        public static int Mins15ToInt(string aMinsInit)
        {
            if (String.IsNullOrEmpty(aMinsInit) || aMinsInit.Length!=5 || aMinsInit.Substring(2,1)!="h")
                return -1;
            int h = -1;
            var hres = Int32.TryParse(aMinsInit.Substring(0, 2), out h);
            int m = -1;
            var mres = Int32.TryParse(aMinsInit.Substring(3, 2), out m);
            if (!mres || !hres || m<0 || h<0 || m>59 || h>23)
                return -1;           
            var mins = Int32.Parse(aMinsInit.Substring(3, 2)) + 60*Int32.Parse(aMinsInit.Substring(0, 2));    
            return mins;
        }

        public static string TimeFrToAn(string aDebutMinsPoint)
        {

            var res = aDebutMinsPoint;
            res = res.Replace("h", ":");
            res = res.Replace("H", ":");
            if (res.Length != 5)
                return res;
            if (res.Substring(2, 1) != ":")
                return res;

            int h = -1;
            var hres = Int32.TryParse(res.Substring(0, 2), out h);
            if (h == -1)
                return res;
            if (h < 10)
            {
                res = res.Substring(1) + " AM";
                return res;
            }
            if (h >= 10 && h < 12)
            {
                res = res + " AM";
                return res;
            }
            if (h >= 12)
            {
                if (h >= 13)
                {
                    res = (h - 12).ToString() + res.Substring(2);
                }
                res = res + " PM";
                return res;
            }

            return res;

        }

        public static bool IsValidEmail(string aEmail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(aEmail);
                return addr.Address == aEmail;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidCell(string aCell)
        {
            try
            {
                var res = !String.IsNullOrEmpty(aCell);
                if (res)
                {
                    res = res && aCell.Length == 10;
                }
                if (res)
                {
                    Regex rgNumber = new Regex("[0-9]");
                    res = res && rgNumber.Matches(aCell).Count == 10;
                }
                return res;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidPassword(string aPassword)
        {
            try
            {
                if (String.IsNullOrEmpty(aPassword))
                {
                    return false;
                }
                Regex rgUpper = new Regex("[A-Z]");
                Regex rgLower = new Regex("[a-z]");
                Regex rgNumber = new Regex("[0-9]");
                Regex rgSpecial = new Regex("[^a-zA-Z0-9]");
                if (aPassword.Length > 7 &&
                    rgUpper.Matches(aPassword).Count > 0 &&
                    rgLower.Matches(aPassword).Count > 0 &&
                    rgNumber.Matches(aPassword).Count > 0 &&
                    rgSpecial.Matches(aPassword).Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static string GenerateAlphaNumPasswordComplexe(int length)
        {
            if (length < 8 || length > 100)
                throw new ArgumentOutOfRangeException("La valeur doit être comprise entre 8 et 100");
            string res = "";

            Random random = new Random();

            List<char> charList = new List<char>();

            for (int i = 0; i < length; i++)
            {
                var isNum = i % 8;
                if (isNum == 0 || isNum == 1)
                {
                    charList.Add((char)random.Next(65, 90));
                }
                if (isNum == 2 || isNum == 3 || isNum == 4)
                {
                    charList.Add((char)random.Next(97, 122));
                }
                if (isNum == 5 || isNum == 6)
                {
                    charList.Add((char)random.Next(48, 57));
                }
                if (isNum == 7)
                {
                    var no = random.Next(0, 1);
                    if (no == 0)
                    {
                        charList.Add((char)random.Next(35, 38));
                    }
                    else
                    {
                        charList.Add((char)random.Next(40, 43));
                    }

                }
            }

            List<int> position = Enumerable.Range(0, length).ToList();

            while (charList.Count > 0)
            {
                var no = random.Next(0, charList.Count);
                var i = position[no];
                res += charList[i];
                charList.RemoveAt(i);
            }

            return res;
        }

        public static string ReplaceCharFromXML(string aInput)
        {
            var res = aInput;
            res = res.Replace("&", "&amp;");
            res = res.Replace("<", "&#60;");
            res = res.Replace(">", "&#62;");
            res = res.Replace("'", "&#39;");
            res = res.Replace(@"""", "&#39;");
            return res;
        }

        public static string CorrectForAddressCDK(string aInput)
        {
                char[] allowed = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-&# /".ToCharArray();
                char[] charArray = aInput.ToString().ToCharArray();
                StringBuilder result = new StringBuilder();
                foreach (char c in charArray)
                {
                    foreach (char a in allowed)
                    {
                        if(c==a) result.Append(a);
                    }
                }
                return result.ToString();

            //var stringOut = Regex.Replace(aInput, "[^A-Za-z0-9][^.][^&amp;][^#][^-][^/]", "");
            //return stringOut;
        }

    }
}
