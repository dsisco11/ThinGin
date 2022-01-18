
using System;

using ThinGin.Core.Exceptions;

namespace ThinGin.OpenGL.Common.Exceptions
{
    /// <summary>
    /// An exception representing an generic error
    /// </summary>
    public class OpenGLException : ThinGinException
    {
        public OpenGLException()
        {
        }

        public OpenGLException(string message) : base(string.Concat("[OpenGL] ", message))
        {
        }

        public OpenGLException(string message, Exception innerException) : base(string.Concat("[OpenGL] ", message), innerException)
        {
        }
    }
}
