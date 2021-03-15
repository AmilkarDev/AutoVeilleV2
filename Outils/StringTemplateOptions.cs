using System;

namespace Outils
{
    /// <summary>
    /// Options that affect the template loading behavior.
    /// </summary>
    [Flags]
    public enum StringTemplateOptions
    {
        /// <summary>
        /// No options are specified.  Use defaults everywhere.
        /// </summary>
        None = 0,

        /// <summary>
        /// Flush all leading and trailing blanks on template lines.  This produces a more compact result.
        /// </summary>
        TrimBlanks = 1,

        /// <summary>
        /// Do not proceed with variable substitutions.  Perform only inclusions.
        /// </summary>
        InclusionsOnly = 2
    }
}
