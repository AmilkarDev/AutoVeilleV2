using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models.Web
{
    public class UtilisateurSite
    {

        public int UserID { get; set; }
        public int NoCommerce { get; set; }
        public string NomCommerce { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public int TypeUsager { get; set; }
    }
}
