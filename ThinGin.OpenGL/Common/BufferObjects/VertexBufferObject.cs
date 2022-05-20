using ThinGin.Core.Common.Interfaces;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.BufferObjects
{
    /// <summary>
    /// Manages a buffer of vertex data
    /// </summary>
    public class VertexBufferObject : GLBufferObject
    {
        #region Values
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public override BufferTarget Target => BufferTarget.ArrayBuffer;
        #endregion

        #region Constructors
        public VertexBufferObject(EngineInstance Engine) : base(Engine)
        {
            if (!Engine.IsSupported("arb_vertex_buffer_object"))
            {
                throw new Exceptions.OpenGLUnsupportedException(nameof(VertexBufferObject), "The vertex buffer object extension is not supported by the graphics driver!");
            }
        }
        #endregion

    }
}
