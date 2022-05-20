namespace ThinGin.Core.Console.Parsing
{
    public class BadUrlToken : UrlToken
    {
        public BadUrlToken() : base(string.Empty, ECTokenType.Bad_Url)
        {
        }

        public override string Encode() { return string.Empty; }
    }
}
