using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class ParenthesisCloseToken : CToken
    {
        public static ParenthesisCloseToken Instance = new ParenthesisCloseToken();
        public ParenthesisCloseToken() : base(ECTokenType.Parenth_Close)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_RIGHT_PARENTHESES, 1);
    }
}
