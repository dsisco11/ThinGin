using System;
using System.Text;

namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// Represents an indice range with a start and end index.
    /// <notes>Format: [start:end]</notes>
    /// </summary>
    public sealed class RangeToken : CToken
    {
        public readonly Range Value;

        public RangeToken(Range value) : base(ECTokenType.Indice_Range)
        {
            Value = value;
        }

        public override string Encode()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            if (Value.Start.IsFromEnd) sb.Append("-");
            sb.Append(Value.Start.Value);

            sb.Append(":");

            if (Value.End.IsFromEnd) sb.Append("-");
            sb.Append(Value.End.Value);

            sb.Append("]");
            return sb.ToString();
        }

        #region Equality Operators
        public override bool Equals(object o)
        {
            if (o is RangeToken Other)
            {
                return Type == Other.Type && Value.Equals(Other.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
            //return HashCode.Combine(Start, End);
        }

        #endregion
    }
}
