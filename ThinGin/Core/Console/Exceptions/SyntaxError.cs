using System;

using ThinGin.Core.Common.Types;
using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console
{
    public class SyntaxError : ParserException
    {
        static string E_MSG = "Syntax error!";

        public SyntaxError() : base(E_MSG)
        {
        }

        public SyntaxError(DataConsumer<CToken> Stream) : base(string.Concat(E_MSG, " @\"", ParsingCommon.Get_Location_String(Stream.AsSpan()), "\""))
        {
        }

        public SyntaxError(DataConsumer<CToken> Stream, string message) : base(string.Concat(message, " @\"", ParsingCommon.Get_Location_String(Stream.AsSpan()), "\""))
        {
        }

        public SyntaxError(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SyntaxError(DataConsumer<CToken> Stream, string message, Exception innerException) : base(string.Concat(message, " @\"", ParsingCommon.Get_Location_String(Stream.AsSpan()), "\""), innerException)
        {
        }
    }
}
