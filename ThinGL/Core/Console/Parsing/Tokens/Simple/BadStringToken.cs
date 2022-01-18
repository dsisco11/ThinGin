using System;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class BadStringToken : ValuedTokenBase
    {
        public BadStringToken(ReadOnlySpan<char> Value) : base(ECTokenType.Bad_String, Value, false)
        {
        }

        public override string Encode() { return string.Empty; }
    }
}
