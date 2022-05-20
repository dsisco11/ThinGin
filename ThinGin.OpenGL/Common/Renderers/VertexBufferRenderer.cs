
using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Exceptions;
using ThinGin.Core.Rendering.Common;
using ThinGin.OpenGL.Common.BufferObjects;

namespace ThinGin.OpenGL.Common.Renderers
{
    public class VertexBufferRenderer : MeshRenderer
    {
        #region Properties
        protected VertexBufferObject VBO = null;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public VertexBufferRenderer(EngineInstance Engine, Mesh mesh) : base(Engine, mesh)
        {
        }
        #endregion

        #region Disposal

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (VBO is object)
            {
                VBO.Dispose();
                VBO = null;
            }
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer()
        {
            return new EngineDelegate(() =>
            {
                VBO = new VertexBufferObject(Engine);
                VBO.Upload(mesh.Data, null);
            });
        }
        public override IEngineDelegate Get_Releaser() => null;

        public override IEngineDelegate Get_Updater()
        {
            return new EngineDelegate(() =>
            {
                VBO.Upload(mesh.Data, null);
#if DEBUG
                Engine.ErrorCheck(out _);
#endif
            });
        }
        #endregion

        #region Rendering
        public override void Render(ETopology Primitive)
        {
            var Layout = mesh.Layout;

            VBO.Bind();
            unsafe
            {
                if (Layout.Has(EVertexAttribute.Position))
                {
                    GL.EnableClientState(ArrayCap.VertexArray);
                    var attrib = Layout.Get(EVertexAttribute.Position);
                    var pty = Common.Bridge.Translate(attrib.ValueType);
                    GL.VertexPointer(attrib.Count, (VertexPointerType)pty, Layout.Size, attrib.Offset);
                }

                if (Layout.Has(EVertexAttribute.Normal))
                {
                    GL.EnableClientState(ArrayCap.NormalArray);
                    var attrib = Layout.Get(EVertexAttribute.Normal);
                    var pty = Common.Bridge.Translate(attrib.ValueType);
                    GL.NormalPointer((NormalPointerType)pty, Layout.Size, attrib.Offset);
                }

                if (Layout.Has(EVertexAttribute.Color))
                {
                    GL.EnableClientState(ArrayCap.ColorArray);
                    var attrib = Layout.Get(EVertexAttribute.Color);
                    var pty = Common.Bridge.Translate(attrib.ValueType);
                    GL.ColorPointer(attrib.Count, (ColorPointerType)pty, Layout.Size, attrib.Offset);
                }

                if (Layout.Has(EVertexAttribute.UVMap))
                {
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                    var attrib = Layout.Get(EVertexAttribute.UVMap);
                    var pty = Common.Bridge.Translate(attrib.ValueType);
                    GL.TexCoordPointer(attrib.Count, (TexCoordPointerType)pty, Layout.Size, attrib.Offset);
                }
            }

            var primitive = Common.Bridge.Translate(Primitive);
            GL.DrawArrays(primitive, 0, mesh.Count);

            VBO.Unbind();
        }
        #endregion
    }
}
