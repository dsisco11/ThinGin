using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Types;

namespace ThinGin.Core.Console.Parsing
{
    public class TokenStream : DataConsumer<CToken>
    {
        #region Constructors
        public TokenStream(ReadOnlyMemory<CToken> Tokens) : base(Tokens.ToArray())
        {
        }

        public TokenStream(IEnumerable<CToken> Tokens) : base(new List<CToken>(Tokens).ToArray())
        {
        }
        #endregion
    }
}
