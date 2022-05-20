using System;

using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common
{
    public enum EPixelBufferMode { Pack, Unpack }
    /// <summary>
    /// Implements functionality for interacting with OpenGL pixelbuffers for Async data transfer via DMA(Direct memory access)
    /// </summary>
    public class GLPixelBuffer : GLBufferObject
    {
        #region Values
        private readonly BufferTarget _target;
        private readonly EPixelBufferMode _mode;
        #endregion

        #region Accessors
        public override BufferTarget Target => _target;
        #endregion

        #region Constructors
        /// <summary>
        /// instantiates a new pixel buffer data block on the GPU
        /// </summary>
        /// <param name="Mode"></param>
        /// <param name="Length">byte length of buffer</param>
        public GLPixelBuffer(EngineInstance engine, EPixelBufferMode Mode, int Length) : base(engine)
        {
            if (!engine.IsSupported("arb_pixel_buffer_object"))
            {
                throw new Exceptions.OpenGLUnsupportedException(nameof(GLPixelBuffer));
            }

            _mode = Mode;
            _length = Length;

            _target = _mode switch
            {
                EPixelBufferMode.Pack => BufferTarget.PixelPackBuffer,
                EPixelBufferMode.Unpack => BufferTarget.PixelUnpackBuffer,
                _ => throw new NotImplementedException(),
            };
        }
        #endregion
    }
}
