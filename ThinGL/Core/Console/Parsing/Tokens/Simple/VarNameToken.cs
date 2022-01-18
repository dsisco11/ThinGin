
using System;

using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class VarNameToken : ValuedTokenBase
    {
        public VarNameToken(ReadOnlySpan<char> Value) : base(ECTokenType.VarName, Value, AutoLowercase: false)
        {
        }

        public override string Encode()
        {
            return string.Concat(UnicodeCommon.CHAR_DOLLAR_SIGN, Value);
        }
    }
}
