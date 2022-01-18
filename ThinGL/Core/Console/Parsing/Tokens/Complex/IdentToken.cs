namespace ThinGin.Core.Console.Parsing
{
    public sealed class IdentToken : ValuedTokenBase
    {
        public IdentToken(string Value) : base(ECTokenType.Ident, Value)
        {
        }

        public override string Encode()
        {
            return Value;
        }
    }
}
