namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// Represents a token within an <see cref="CmdTokenizer"/> instance.
    /// </summary>
    public abstract class CToken
    {
        #region Static
        public static EOFToken EOF = EOFToken.Instance;
        #endregion

        #region Properties
        public readonly ECTokenType Type = ECTokenType.None;
        #endregion

        #region Constructors
        public CToken(ECTokenType Type)
        {
            this.Type = Type;
        }
        #endregion

        #region Equality Operators
        public static bool operator ==(CToken left, CToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CToken left, CToken right)
        {
            return !(left == right);
        }

        public override bool Equals(object o)
        {
            if (o is CToken other)
            {
                return Type == other.Type;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Encode().GetHashCode(System.StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        /// <summary>
        /// Encodes the token back to it's representation as a text string
        /// </summary>
        /// <returns>Text form of the token</returns>
        public abstract string Encode();

        #region ToString
        public override string ToString()
        {
            return Encode();
        }
        #endregion
    }
}
