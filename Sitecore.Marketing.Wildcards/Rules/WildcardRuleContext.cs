using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Rules;
using Sitecore.Sites;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class WildcardRuleContext:RuleContext
    {
        public WildcardRuleContext(Item item, SiteContext site)
        {
            this.Item = item;
            this.Site = site;
            this.Tokens = new Dictionary<int, string>();
        }

        public SiteContext Site { get; private set; }
        public int WildcardCount
        { 
            get
            {
                var count = this.Item.Paths.FullPath.Count(s => s == '*');
                return count;
            }
        }

        public WildcardTokenizedString TokenizedString {
            get
            {
                using (new SiteContextSwitcher(this.Site))
                {
                    var url = LinkManager.GetItemUrl(this.Item);
                    var ts = new WildcardTokenizedString(url, this.Tokens);
                    return ts;
                }
            }
        }
        
        public Dictionary<int, string> Tokens { get; private set; }
    }
}
