using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;


namespace Outils.Security
{
    public class UserSecurityInfo
    {
        public String Name { get; set; }

        public String Password { get; set; }

        public static   WindowsIdentity CurrentWindowsUser
        {
            get { return WindowsIdentity.GetCurrent(); }
        }

        public static String CurrentWindowsUserName
        {
            get { return CurrentWindowsUser.Name.Replace(@"SULY\", ""); }
        }

        public static Dictionary<GroupePotPerdu, Boolean> GetGroupsPotPerduCurrentWindowsUser()
        {
           
            var groupPotPerdu=new Dictionary<GroupePotPerdu,Boolean>();
            var sid = CurrentWindowsUser.Groups.ToList();
            var groupNames = new List<String>();
            foreach (var s in sid)
            {
                try
                {
                    NTAccount ntAccount = (NTAccount)s.Translate(typeof(NTAccount));
                    var nom = ntAccount.ToString();

                    if (nom.Substring(0, Math.Min(13, nom.Length)) == "SULY\\PotPerdu")
                    {
                        groupNames.Add(nom);

                    }
                }
                catch (Exception ex)
                {

                }

            }
            groupPotPerdu[GroupePotPerdu.Admin] = groupNames.Any(x => x == "SULY\\PotPerdu_Admin");
            groupPotPerdu[GroupePotPerdu.AdminCentreDAppel] = groupNames.Any(x => x == "SULY\\PotPerdu_AdminCentreDAppel");
            groupPotPerdu[GroupePotPerdu.CentreAAppel] = groupNames.Any(x => x == "SULY\\PotPerdu_CentreDAppel");
            groupPotPerdu[GroupePotPerdu.AdminProduction] = groupNames.Any(x => x == "SULY\\PotPerdu_AdminProduction");
            groupPotPerdu[GroupePotPerdu.Production] = groupNames.Any(x => x == "SULY\\PotPerdu_Production");
      

            //    groupsAutoveille[GroupADAutoveille.Autoveille_administration] =
            //             groupNames.Any(x =>
            //                     x== "SULY\\Autoveille_administration");

            //    groupsAutoveille[GroupADAutoveille.Autoveille_developpement] =
            //            groupNames.Any(x =>
            //                    x == "SULY\\Autoveille_developpement");

            //    groupsAutoveille[GroupADAutoveille.Autoveille_Production] =
            //            groupNames.Any(x =>
            //                    x== "SULY\\Autoveille_Production");

            //    groupsAutoveille[GroupADAutoveille.Autoveille_SuperviseursCentreAppel] =
            //            groupNames.Any(x =>
            //                    x == "SULY\\Autoveille_SuperviseursCentreAppel");

            //    groupsAutoveille[GroupADAutoveille.Autoveille_SuperviseursProduction] =
            //            groupNames.Any(x =>
            //                    x== "SULY\\Autoveille_SuperviseursProduction");

            return groupPotPerdu;
        }
        public static Dictionary<GestionAAS, Boolean> GetGroupsAASCurrentWindowsUser()
        {

            var groupAAS = new Dictionary<GestionAAS, Boolean>();
            var sid = CurrentWindowsUser.Groups.ToList();
            var groupNames = new List<String>();
            foreach (var s in sid)
            {
                try
                {
                    NTAccount ntAccount = (NTAccount)s.Translate(typeof(NTAccount));
                    var nom = ntAccount.ToString();

                    if (nom.Substring(0, Math.Min(15, nom.Length)) == "SULY\\GestionAAS")
                    {
                        groupNames.Add(nom);

                    }
                }
                catch (Exception ex)
                {

                }

            }
            groupAAS[GestionAAS.Admin] = groupNames.Any(x => x == "SULY\\GestionAAS");




            return groupAAS;
        }
        //public static Dictionary<GroupADAutoveille, Boolean>  GetGroupsAutoveilleCurrentWindowsUser()
        //{
        //   var groupsAutoveille=new Dictionary<GroupADAutoveille, Boolean>();


        //   var sid = CurrentWindowsUser.Groups.ToList();
        //    var groupNames = new List<String>();
           
        //   foreach (var s in sid)
        //   {
        //       try {
        //            NTAccount ntAccount = (NTAccount)s.Translate(typeof(NTAccount));
        //            var nom = ntAccount.ToString();
                   
        //            if (nom.Substring(0, Math.Min(15, nom.Length)) == "SULY\\Autoveille")
        //                {
        //                   groupNames.Add(nom);
                           
        //                }
        //           }
        //    catch (Exception ex)
        //    {
               
        //    }
        //   }




       //    groupsAutoveille[GroupADAutoveille.Autoveille_CentreAppel] =
       //groupNames.Any(x =>
       //         x == "SULY\\Autoveille_CentreAppel");

       //    groupsAutoveille[GroupADAutoveille.Autoveille_administration] =
       //             groupNames.Any(x =>
       //                     x== "SULY\\Autoveille_administration");

       //    groupsAutoveille[GroupADAutoveille.Autoveille_developpement] =
       //            groupNames.Any(x =>
       //                    x == "SULY\\Autoveille_developpement");

       //    groupsAutoveille[GroupADAutoveille.Autoveille_Production] =
       //            groupNames.Any(x =>
       //                    x== "SULY\\Autoveille_Production");

       //    groupsAutoveille[GroupADAutoveille.Autoveille_SuperviseursCentreAppel] =
       //            groupNames.Any(x =>
       //                    x == "SULY\\Autoveille_SuperviseursCentreAppel");

       //    groupsAutoveille[GroupADAutoveille.Autoveille_SuperviseursProduction] =
       //            groupNames.Any(x =>
       //                    x== "SULY\\Autoveille_SuperviseursProduction");




       //    return groupsAutoveille ;
       // }

        public static String GetCurrentUserEmail()
        {
            // get a DirectorySearcher object
            var search = new DirectorySearcher();

            // specify the search filter
            var user = CurrentWindowsUserName.Substring(CurrentWindowsUserName.LastIndexOf('\\')+1);
            search.Filter = String.Format("(&(objectClass=user)(anr={0}))", user);

            // specify which property values to return in the search
            search.PropertiesToLoad.Add("givenName");   // first name
            search.PropertiesToLoad.Add("sn");          // last name
            search.PropertiesToLoad.Add("mail");        // smtp mail address

            // perform the search
            SearchResult result = search.FindOne();
            if (result.Properties["mail"].Count > 0)
                return result.Properties["mail"][0] as string;

            return null;
        }


        public static Tuple<string, string, string> GetCurrentUserInfos()
        {
            // get a DirectorySearcher object
            var search = new DirectorySearcher();

            // specify the search filter
            
            var user = CurrentWindowsUserName.Substring(CurrentWindowsUserName.LastIndexOf('\\') + 1);
            search.Filter = String.Format("(&(objectClass=user)(anr={0}))", user);

            // specify which property values to return in the search
            search.PropertiesToLoad.Add("givenName");   // first name
            search.PropertiesToLoad.Add("sn");          // last name
            //search.PropertiesToLoad.Add("mail");        // smtp mail address

            // perform the search
            SearchResult result = search.FindOne();
            if (result.Properties["givenName"].Count > 0 && result.Properties["sn"].Count > 0)
            {
                var nom = result.Properties["givenName"][0] == null ? "" : (result.Properties["givenName"][0]).ToString();
                var prenom = result.Properties["sn"][0] == null ? "" : (result.Properties["sn"][0]).ToString();
                return new Tuple<string, string, string>(nom, prenom, user);
            } 

            return null;
        }

        public static String GetCurrentUserInit()
        {
            // get a DirectorySearcher object
            var search = new DirectorySearcher();

            // specify the search filter
            var user = CurrentWindowsUserName.Substring(CurrentWindowsUserName.LastIndexOf('\\') + 1);
            search.Filter = String.Format("(&(objectClass=user)(anr={0}))", user);

            // specify which property values to return in the search
            search.PropertiesToLoad.Add("givenName");   // first name
            search.PropertiesToLoad.Add("sn");          // last name
            search.PropertiesToLoad.Add("mail");        // smtp mail address

            // perform the search
            SearchResult result = search.FindOne();
            if (result.Properties["givenName"].Count > 0 && result.Properties["sn"].Count > 0)
            {
                var res = result.Properties["givenName"][0]==null?"":(result.Properties["givenName"][0] as string).Substring(0, 1);
                res = res +( result.Properties["sn"][0] == null ? "" : (result.Properties["sn"][0] as string).Substring(0, 1));
                return res.ToUpper();
            } 
               

            return user;
        }
    }
}
