using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoveilleBL.Models;

namespace Autoveille.Models
{
    public class Appels
    {
        public List<Relance> Relances { get; set; }
        public Evenement Evenement { get; set; }
    }
}