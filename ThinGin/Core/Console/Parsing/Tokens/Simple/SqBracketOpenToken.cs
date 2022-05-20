using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class SqBracketOpenToken : CToken
    {
        public static SqBracketOpenToken Instance = new SqBracketOpenToken();
        public SqBracketOpenToken() : base(ECTokenType.SqBracket_Open)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_LEFT_SQUARE_BRACKET, 1);
    }
}
