using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models
{
    public class Concession
    {
        public int NoCommerce { get; set; }
        public string NomCommerce { get; set; }
        public string NomCommerceComplet
        {
            get
            {
                return NoCommerce.ToString() + " - " + NomCommerce;
            }
            
        }
    }
}
