using System;

namespace ThinGin.Core.Console
{
    public class VConsoleException : Exception
    {
        public VConsoleException()
        {
        }

        public VConsoleException(string message) : base(message)
        {
        }

        public VConsoleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
