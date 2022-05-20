using System;
using System.Linq;

namespace ThinGin.Core.Console.Parsing
{
    public static class ParsingCommon
    {
        #region Utility
        public static string Get_Location_String(ReadOnlySpan<CToken> Stream)
        {
            var tokens = from tok in Stream.Slice(0, 6).ToArray() select tok.Encode();
            return string.Join("", tokens);
        }
        #endregion
    }
}
