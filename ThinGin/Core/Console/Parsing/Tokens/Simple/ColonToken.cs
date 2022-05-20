
using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class ColonToken : CToken
    {
        public static ColonToken Instance = new ColonToken();
        public ColonToken() : base(ECTokenType.Colon)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_COLON, 1);
    }
}
