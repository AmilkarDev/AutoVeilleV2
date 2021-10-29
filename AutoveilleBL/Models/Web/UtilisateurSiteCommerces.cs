using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models.Web
{
    public class UtilisateurSiteCommerces
    {
        public int UserId { get; set; }
        public int NoCommerce { get; set; }
        public string NomCommerce { get; set; }
        public Roles Role { get; set; }
        public UserTypes TypeUsager { get; set; }

        public string Titre { get; set; }

    }
}
