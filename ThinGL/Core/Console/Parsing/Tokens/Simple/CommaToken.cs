using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class CommaToken : CToken
    {
        public static CommaToken Instance = new CommaToken();
        public CommaToken() : base(ECTokenType.Comma)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_COMMA, 1);
    }
}
