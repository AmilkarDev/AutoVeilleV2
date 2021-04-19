using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models
{
    public class ListeAppels    {
        public int IdEvenement { get; set; }
        public int NoCommerce { get; set; }
        public int TypeAppel { get; set; }
        public int OrdreAfichagePortail { get; set; }
        public string DescriptionListe { get; set; }
        public int NbreAppels { get; set; }
        public int NbreRejoints { get; set; }
        public int NbreRDVs { get; set; }
        public int NbreDesactives { get; set; }
        public int NbreNonMercis { get; set; }
        public int NbreLitiges { get; set; }
    }
}
