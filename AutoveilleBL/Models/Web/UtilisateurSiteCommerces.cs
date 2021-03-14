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
        public int Role { get; set; }
        public int TypeUsager { get; set; }
    }
}
