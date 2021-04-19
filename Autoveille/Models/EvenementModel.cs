using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Autoveille.Models
{
    public class EvenementModel
    {
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int IdEvenement { get; set; }
        public int NoCommerce { get; set; }
        public string NomCommerce { get; set; }
        public int NbreAppels { get; set; }
        public int NbreRejoints { get; set; }
        public int NbreARejoints { get; set; }
        public string TypeEvenement
        {
            get
            {
                var dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (DateDebut <= dateNow && DateFin >= dateNow)
                {
                    return "Evenement en cours";
                }
                if (DateDebut > dateNow)
                {
                    return "Evenement à venir";
                }
                if (DateFin < dateNow)
                {
                    return "Evenement passé";
                }
                return "Erreur dates";
            }
        }
   public string DatesEvenementsFr
        {
            get
            {
                CultureInfo fr = new CultureInfo("fr-CA");
                if (DateFin==null && DateDebut ==null)
                {
                    return "Dates d'evenement pas definies!";
                }
                if (DateFin==null && DateDebut!=null)
                {
                    return "début evenement : " +((DateTime) DateDebut).ToString("dd MMMM yyyy",fr) ;
                }
                if (DateFin != null && DateDebut == null)
                {
                    return "fin evenement : " +((DateTime)DateFin).ToString("dd MMMM yyyy", fr);
                }
                if (((DateTime)DateFin).Year!=((DateTime) DateDebut).Year)
                {
                    return ((DateTime) DateDebut).ToString("dd MMMM yyyy", fr) + " au " 
                        +((DateTime)DateFin).ToString("dd MMMM yyyy", fr);
                }
                if (((DateTime)DateFin).Month!=((DateTime)DateDebut).Month)
                {
                    return ((DateTime)DateDebut).ToString("dd MMMM", fr) + " au " 
                        + ((DateTime)DateFin).ToString("dd MMMM yyyy", fr);
                } 
                return ((DateTime) DateDebut).ToString("dd", fr) + " au " 
                    +((DateTime)DateFin).ToString("dd MMMM yyyy", fr);
            }
        }           
    }
}