using System;

namespace ThinGin.Core.Console.Parsing
{
    public class UrlToken : ValuedTokenBase
    {
        #region Constructors
        public UrlToken(ReadOnlySpan<char> Value, ECTokenType Type) : base(Type, Value, false)
        {
        }
        public UrlToken(ReadOnlySpan<char> Value) : base(ECTokenType.Url, Value, false)
        {
        }
        #endregion

        public override string Encode()
        {
            return string.Concat("url(", Value, ")");
        }
    }
}
