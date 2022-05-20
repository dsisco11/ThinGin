using System;

using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class StringToken : ValuedTokenBase
    {
        public StringToken(ReadOnlySpan<char> Value) : base(ECTokenType.String, Value, false)
        {
        }

        public override string Encode()
        {
            return string.Concat(UnicodeCommon.CHAR_QUOTATION_MARK, Value, UnicodeCommon.CHAR_QUOTATION_MARK);
        }
    }
}
