using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// '{'
    /// </summary>
    public sealed class BracketOpenToken : CToken
    {
        public static BracketOpenToken Instance = new BracketOpenToken();
        public BracketOpenToken() : base(ECTokenType.Bracket_Open)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_LEFT_CURLY_BRACKET, 1);
    }
}
