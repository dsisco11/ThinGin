using System;

namespace ThinGin.Core.Console
{
    public class ParserFailedException : ParserException
    {
        static string E_MSG = "Error parsing console command!";

        public ParserFailedException() : base(E_MSG)
        {
        }

        public ParserFailedException(string message) : base(string.Concat(E_MSG, " ", message))
        {
        }

        public ParserFailedException(string message, Exception innerException) : base(string.Concat(E_MSG, " ", message), innerException)
        {
        }
    }
}
