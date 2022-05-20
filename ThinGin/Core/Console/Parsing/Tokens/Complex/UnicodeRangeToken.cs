using System;
using System.Globalization;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class UnicodeRangeToken : CToken
    {
        /// <summary>
        /// Starting point of the range
        /// </summary>
        public readonly int Start;
        /// <summary>
        /// Ending point of the range
        /// </summary>
        public readonly int End;

        public UnicodeRangeToken(int Start, int End) : base(ECTokenType.Unicode_Range)
        {
            this.Start = Start;
            this.End = End;
        }

        public override string Encode()
        {
            string StartRange = Start.ToString("X", CultureInfo.InvariantCulture);
            string EndRange = End.ToString("X", CultureInfo.InvariantCulture);
            return string.Concat("U+", StartRange, "-", "U+", EndRange);
        }

        #region Equality Operators
        public override bool Equals(object o)
        {
            if (o is UnicodeRangeToken Other)
            {
                return Type == Other.Type && Start == Other.Start && End == Other.End;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        #endregion
    }
}
