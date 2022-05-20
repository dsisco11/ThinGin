using System;

using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class PercentageToken : ValuedTokenBase
    {
        /// <summary>
        /// Holds the numeric representation of this token value
        /// </summary>
        public readonly double Number;

        public PercentageToken(ReadOnlySpan<char> Value, double Number) : base(ECTokenType.Percentage, Value)
        {
            this.Number = Number;
        }

        public override string Encode()
        {
            return string.Concat(Value, UnicodeCommon.CHAR_PERCENT);
        }
    }
}
