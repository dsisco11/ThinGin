using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class SqBracketCloseToken : CToken
    {
        public static SqBracketCloseToken Instance = new SqBracketCloseToken();
        public SqBracketCloseToken() : base(ECTokenType.SqBracket_Close)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_RIGHT_SQUARE_BRACKET, 1);
    }
}
