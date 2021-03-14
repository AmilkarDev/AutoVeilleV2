using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class Client
    {
        public int ClientId { get; set; }
        public string NomClient { get; set; }
        public string ModeleVehicule { get; set; }
        public DateTime FinTerme { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
    }
}