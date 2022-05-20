namespace ThinGin.Core.Console.Parsing
{
    public sealed class DelimToken : CToken
    {
        public readonly char Value = (char)0;

        public DelimToken(char Value) : base(ECTokenType.Delim)
        {
            this.Value = Value;
        }

        public override string Encode()
        {
            return Value.ToString();
        }

        #region Equality Operators
        public override bool Equals(object o)
        {
            if (o is DelimToken Other)
            {
                return Type == Other.Type && Value == Other.Value;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        #endregion
    }
}
