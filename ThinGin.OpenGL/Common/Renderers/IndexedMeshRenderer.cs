
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
    public class IndexedMeshRenderer : MeshRenderer
    {
        #region Properties
        protected VertexArrayObject VAO = null;
        protected VertexBufferObject VBO = null;
        protected ElementBufferObject EBO = null;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public IndexedMeshRenderer(IEngine Engine, Mesh mesh) : base(Engine, mesh)
        {
        }
        #endregion

        #region Disposal

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (VAO is object)
            {
                VAO.Dispose();
                VAO = null;
            }

            if (VBO is object)
            {
                VBO.Dispose();
                VBO = null;
            }

            if (EBO is object)
            {
                EBO.Dispose();
                EBO = null;
            }
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer()
        {
            return new EngineDelegate(() =>
            {
                VBO = new VertexBufferObject(RHI);
                VBO.Bind();
                VBO.Upload(mesh.Data, null);

                VAO = new VertexArrayObject(RHI, mesh.Layout);
                VAO.Bind();

                EBO = new ElementBufferObject(RHI);
                EBO.Bind();
                EBO.Upload(mesh.Indices.Data, mesh.Indices.Layout);

                VBO.Unbind();
                VAO.Unbind();
                EBO.Unbind();
            });
        }
        public override IEngineDelegate Get_Releaser() => null;

        public override IEngineDelegate Get_Updater()
        {
            return new EngineDelegate(() =>
            {
                VBO.Upload(mesh.Data, null);
                EBO.Upload(mesh.Indices.Data, mesh.Indices.Layout);
#if DEBUG
                RHI.ErrorCheck(out _);
#endif
            });
        }
        #endregion

        #region Rendering
        public override void Render(ETopology Primitive)
        {
            if (!IsInitialized)
                return;

            RHI.Bind(VAO);

            var primitive = Common.Bridge.Translate(Primitive);
            GL.DrawElements(primitive, EBO.Length, EBO.ElementType, System.IntPtr.Zero);

#if DEBUG
            if (RHI.ErrorCheck(out var msg))
            {
                System.Diagnostics.Debugger.Break();
            }
#endif

            RHI.Unbind(VAO);
        }
        #endregion
    }
}
