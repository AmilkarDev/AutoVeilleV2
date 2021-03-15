using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils.Security
{
    public enum GroupePotPerdu
    {
        [Description("Admin - toutes les doits")]
        Admin = 1,

        [Description("Admin - centre d'appel")]
        AdminCentreDAppel = 2,

        [Description("Agent d'appel")]
        CentreAAppel = 4,

        [Description("Admin - production")]
        AdminProduction = 8,

        [Description("Production")]
        Production = 16,
    }
}
