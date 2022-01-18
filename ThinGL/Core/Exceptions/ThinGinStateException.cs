
using System;
using System.Runtime.Serialization;

namespace ThinGin.Core.Exceptions
{
    /// <summary>
    /// An exception representing an error in state management
    /// </summary>
    public class ThinGinStateException : Exception
    {
        public ThinGinStateException()
        {
        }

        public ThinGinStateException(string message) : base(string.Concat("[ThinGin] ", message))
        {
        }

        public ThinGinStateException(string message, Exception innerException) : base(string.Concat("[ThinGin] ", message), innerException)
        {
        }

        protected ThinGinStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
