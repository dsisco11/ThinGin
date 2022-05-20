using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Types;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Enums;
using ThinGin.OpenGL.Common.Exceptions;
using System;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.BufferObjects
{
    /// <summary>
    /// Manages a buffer of vertex data
    /// </summary>
    public class VertexArrayObject : BufferObject
    {
        #region Properties
        protected VertexLayout Layout;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public VertexArrayObject(EngineInstance engine, VertexLayout Layout) : base(engine)
        {
            if (!engine.Renderer.IsSupported("arb_vertex_array_object"))
            {
                throw new OpenGLException("The Vertex array object extension is not supported by the graphics driver!");
            }

            this.Layout = Layout;
        }
        #endregion

        #region Initialization
        protected void Setup()
        {
            if (!_bind_state) GL.BindVertexArray(_handle);

            unsafe
            {
                for (int attrIndex = 0; attrIndex < Layout.Count; attrIndex++)
                {
                    var attribute = Layout[attrIndex];
                    GL.VertexAttribPointer(attrIndex, attribute.Count, Common.Bridge.Translate(attribute.ValueType), attribute.Normalize, attribute.Stride, attribute.Offset);

                    if (RHI.Compatability.AttributeDivisors && attribute.IndexDivisor > 0)
                    {
                        GL.VertexAttribDivisor(attrIndex, attribute.IndexDivisor);
                    }

                    GL.EnableVertexAttribArray(attrIndex);
                }
            }

            if (!_bind_state) GL.BindVertexArray(0);
        }
        #endregion

        #region Binding
        /// <summary>
        /// <inheritdoc cref="IBufferObject.Bind"/>
        /// </summary>
        /// <returns>Buffer handle</returns>
        public override void Bind()
        {
            EnsureReady();
            GL.BindVertexArray(_handle);
        }

        /// <summary>
        /// <inheritdoc cref="IBufferObject.Unbind"/>
        /// </summary>
        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }
        #endregion

        #region Direct Memory Access
        public override bool TryMap(ERHIAccess Access, out IntPtr dmaAddress)
        {
            throw new NotImplementedException();
        }

        public override bool TryUnmap()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => GL.GenVertexArrays(1, out _handle));
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(Handle, GL.DeleteVertexArray);
        public override IEngineDelegate Get_Updater() => new EngineDelegate(Setup);
        #endregion
    }
}
