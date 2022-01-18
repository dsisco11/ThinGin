namespace ThinGin.Core.Console.Parsing
{
    public class EOFToken : CToken
    {
        public static EOFToken Instance = new EOFToken();
        public EOFToken() : base (ECTokenType.EOF)
        {
        }

        public override string Encode()
        {
            return string.Empty;
        }
    }
}
