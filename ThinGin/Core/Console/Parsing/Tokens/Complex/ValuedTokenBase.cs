using System;

namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// Implements a token type that holds a value comprised on zero or more characters
    /// </summary>
    public abstract class ValuedTokenBase : CToken
    {
        #region Properties
        /// <summary>
        /// Holds the string representation of this tokens value
        /// </summary>
        public readonly string Value = null;
        #endregion

        #region Constructors
        public ValuedTokenBase(ECTokenType Type, ReadOnlySpan<char> Value, bool AutoLowercase = true) : base(Type)
        {
            if (AutoLowercase)
            {
                unsafe
                {
                    var buf = stackalloc char[Value.Length];
                    var spn = new Span<char>(buf, Value.Length);
                    Value.ToLowerInvariant(spn);
                    this.Value = spn.ToString();
                }
            }
            else this.Value = Value.ToString();
        }
        #endregion

        public override bool Equals(object o)
        {
            if (o is ValuedTokenBase other)
            {
                return Type == other.Type && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override string Encode()
        {
            return Value;
        }

    }
}
