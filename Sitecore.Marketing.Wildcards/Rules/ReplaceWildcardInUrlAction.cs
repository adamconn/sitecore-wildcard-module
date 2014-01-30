using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Sitecore.Marketing.Wildcards.Rules
{
    public class ReplaceWildcardInUrlAction<T> : RuleAction<T> where T : WildcardRuleContext
    {
        public int Position { get; set; }
        public string Token { get; set; }

        /// <summary>
        /// This method doesn't actually replace anything. It just adds objects to the Tokens collection.
        /// The ruleContext itself performs the transformation. This is because it cannot be assumed
        /// that tokens are applied in order of their position. For example, if someone replaces token
        /// 1 and then tries to replace token 2, token 1 no longer exists and what was previously token
        /// 2 is now token 1.
        /// </summary>
        /// <param name="ruleContext"></param>
        public override void Apply([NotNull] T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            Assert.IsNotNull(ruleContext.Item, "ruleContext.Item");
            Assert.IsNotNull(ruleContext.Tokens, "ruleContext.Tokens");
            //
            //get the token value
            var tokenItem = Sitecore.Context.Database.GetItem(new ID(this.Token));
            Assert.IsNotNull(tokenItem, "The token item {0} cannot be found in the database.", this.Token);
            var valueField = tokenItem.Fields["Value"];
            var tokenValue = tokenItem.Name;
            if (valueField != null && valueField.HasValue)
            {
                tokenValue = valueField.Value;
            }
            ruleContext.Tokens.Add(this.Position, tokenValue);
        }
    }
}