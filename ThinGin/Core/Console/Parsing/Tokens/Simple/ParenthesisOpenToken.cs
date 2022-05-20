using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class ParenthesisOpenToken : CToken
    {
        public static ParenthesisOpenToken Instance = new ParenthesisOpenToken();
        public ParenthesisOpenToken() : base(ECTokenType.Parenth_Open)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_LEFT_PARENTHESES, 1);
    }
}
