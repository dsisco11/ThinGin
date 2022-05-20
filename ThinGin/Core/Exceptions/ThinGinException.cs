
using System;

namespace ThinGin.Core.Exceptions
{
    /// <summary>
    /// An exception representing an generic error
    /// </summary>
    public class ThinGinException : Exception
    {
        public ThinGinException()
        {
        }

        public ThinGinException(string message) : base(string.Concat("[ThinGin] ", message))
        {
        }

        public ThinGinException(string message, Exception innerException) : base(string.Concat("[ThinGin] ", message), innerException)
        {
        }
    }
}
