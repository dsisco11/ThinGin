namespace ThinGin.OpenGL.Common.Exceptions
{
    public class OpenGLUnsupportedException : OpenGLException
    {
        public OpenGLUnsupportedException()
        {
        }

        public OpenGLUnsupportedException(string featureName) : base($"[OpenGL] The currently utilized version of opengl does not support {featureName}!")
        {
        }

        public OpenGLUnsupportedException(string featureName, string message) : base(string.Concat("[", featureName, "] ", message))
        {
        }
    }
}
