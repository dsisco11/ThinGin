
using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class AtToken : CToken
    {
        public static AtToken Instance = new AtToken();

        public AtToken() : base(ECTokenType.At_Sign)
        {
        }

        public override string Encode()
        {
            return new string(UnicodeCommon.CHAR_AT_SIGN, 1);
        }
    }
}
