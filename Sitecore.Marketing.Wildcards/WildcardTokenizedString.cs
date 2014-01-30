using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sitecore.Marketing.Wildcards
{
    public class WildcardTokenizedString:TokenizedString<Dictionary<int, string>>
    {
        public WildcardTokenizedString(string value, Dictionary<int, string> tokens) : base(value, tokens)
        {
        }
        
        protected virtual void Tokenize()
        {
            var transformed = new StringBuilder();
            var pieces = Regex.Split(this.ValueBeforeReplace, Sitecore.Marketing.Wildcards.Configuration.Settings.Tokenize.WildcardTokenizedString);
            for (var i = 0; i < pieces.Length; i++)
            {
                if (this.Tokens.ContainsKey(i))
                {
                    transformed.Append(this.Tokens[i]);
                }
                transformed.Append(pieces[i]);
            }
            _valueAfterReplace = transformed.ToString();
        }

        private string _valueAfterReplace;
        public override string ValueAfterReplace
        {
            get
            {
                if (string.IsNullOrEmpty(_valueAfterReplace))
                {
                    Tokenize();
                }
                return _valueAfterReplace; 
            }
        }
        
        public override bool HasTokens
        {
            get { return this.Tokens.Keys.Count > 0; }
        }

        public override NameValueCollection FindTokenValues(string value)
        {
            var collection = new NameValueCollection();
            var regexValue = this.ValueAfterReplace;
            foreach (var key in this.Tokens.Keys.OrderBy(p => p))
            {
                var token = this.Tokens[key];
                regexValue = regexValue.Replace(token, @"([\S\s]*)");
            }
            var regex = new Regex(regexValue);
            var match = regex.Match(value);
            if (match.Success)
            {
                for (var i = 0; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Value == value)
                    {
                        continue;
                    }
                    if (this.Tokens.ContainsKey(i))
                    {
                        var token = this.Tokens[i];
                        var groupValue = match.Groups[i].Value;
                        collection.Add(token, groupValue);
                    }
                }
            }
            return collection;
        }

    }
}
