
using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Rendering.Common;
using ThinGin.OpenGL.Common.BufferObjects;

namespace ThinGin.OpenGL.Common.Renderers
{
    public class VertexArrayRenderer : MeshRenderer
    {
        #region Properties
        protected VertexArrayObject VAO = null;
        protected VertexBufferObject VBO = null;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public VertexArrayRenderer(IEngine Engine, Mesh mesh) : base(Engine, mesh)
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

            if (VAO is object)
            {
                VAO.Dispose();
                VAO = null;
            }
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer()
        {
            return new EngineDelegate(() =>
            {
                VBO = new VertexBufferObject(RHI);
                VBO.Upload(mesh.Data, null);
                VBO.Bind();

                VAO = new VertexArrayObject(RHI, mesh.Layout);
                VAO.Bind();

                VAO.Unbind();
                VBO.Unbind();
            });
        }
        public override IEngineDelegate Get_Releaser() => null;

        public override IEngineDelegate Get_Updater()
        {
            return new EngineDelegate(() =>
            {
                VBO.Upload(mesh.Data, null);
#if DEBUG
                RHI.ErrorCheck(out _);
#endif
            });
        }
        #endregion

        #region Rendering
        public override void Render(ETopology Primitive)
        {
            if (EnsureReady())
            {
                VAO.Bind();

                var primitive = Common.Bridge.Translate(Primitive);
                GL.DrawArrays(primitive, 0, mesh.Count);

                VAO.Unbind();
            }
        }
        #endregion
    }
}
