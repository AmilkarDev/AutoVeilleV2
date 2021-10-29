using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AutoveilleBL.Models.Enumerations
{
    [Flags]
    public enum s
    {

        [Description("Autoveille v2")]
        AutoveilleV2 = 8,

        [Description("Autoveille financement et comptant")]
        Autoveille = 4,

        [Description("Location")]
        Location = 2,

        [Description("Service")]
        Service = 1,
    }
}
