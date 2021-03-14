using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class NomAppel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int NbrProspects { get; set; }
        public int NbrRejoints { get; set; }
        public int NbrArejoindre { get; set; }
        public int NbrRDV { get; set; }
        public int NbrDesactivee { get; set; }
        public int NbrRelances { get; set; }
        public int NbrLitiges { get; set; }
    }
}