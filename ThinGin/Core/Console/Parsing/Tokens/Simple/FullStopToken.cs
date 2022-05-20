
using ThinGin.Core.Common;

namespace ThinGin.Core.Console.Parsing
{
    public sealed class FullStopToken : CToken
    {
        public static FullStopToken Instance = new FullStopToken();
        public FullStopToken() : base(ECTokenType.FullStop)
        {
        }

        public override string Encode() => new string(UnicodeCommon.CHAR_FULL_STOP, 1);
    }
}
