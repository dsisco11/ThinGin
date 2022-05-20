using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class SemicolonToken : CToken
    {
        public static SemicolonToken Instance = new SemicolonToken();

        public SemicolonToken() : base(ECTokenType.Semicolon)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_SEMICOLON, 1);
    }
}
