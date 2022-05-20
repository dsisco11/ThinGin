
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.OpenGL.Common.Renderers;

namespace ThinGin.OpenGL.GL2.Providers
{
    public class GL2RendererProvider : IRendererProvider
    {
        #region Static Properties
        public static IRendererProvider Instance = new GL2RendererProvider();
        #endregion

        public IMeshRenderer Create(EngineInstance Engine, Mesh mesh)
        {
            return new VertexArrayRenderer(Engine, mesh);
        }
    }
}
