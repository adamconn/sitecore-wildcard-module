using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Marketing.Wildcards.Configuration
{
    public static class Settings
    {
        public static class Tokenize
        {
            public static string WildcardTokenizedString { get { return Sitecore.Configuration.Settings.GetSetting("WildcardTokenizedString", ",-w-,"); } }
        }
    }
}
