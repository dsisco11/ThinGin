
using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    /// <summary>
    /// Represents '}'
    /// </summary>
    public sealed class BracketCloseToken : CToken
    {
        public static BracketCloseToken Instance = new BracketCloseToken();
        public BracketCloseToken() : base(ECTokenType.Bracket_Open)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_RIGHT_CURLY_BRACKET, 1);
    }
}
