using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.OpenGL.Common.RenderHardware
{
    public class RHIOpenGLResourceHandle : RHIHandle
    {
        #region Values
        public readonly int Value;
        #endregion

        #region Constructors
        public RHIOpenGLResourceHandle(int value)
        {
            Value = value;
        }
        #endregion
    }
}
