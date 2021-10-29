using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models.Web
{
    public enum UserTypes  {Suly ,Concessionnaire  };
    public enum Roles { Gestionnaire, Consultant, Concessionnaire }
    public enum Langues {   Anglais, Français }
    public class UtilisateurSite
    {

        public int UserID { get; set; }
        public int NoCommerce { get; set; }
        public string NomCommerce { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Langues  Langue { get; set; }
        public Roles Role { get; set; }
        public UserTypes TypeUsager { get; set; }
    }
}
