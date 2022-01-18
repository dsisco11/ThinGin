using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.OpenGL.Common.Providers;
using ThinGin.OpenGL.GL2.Providers;

namespace ThinGin.OpenGL.GL2
{
    public sealed class Implementation : IGraphicsImplementation
    {
        #region Static Instance
        public static IGraphicsImplementation Instance = new Implementation();
        #endregion

        #region Providers
        public IEngineProvider Engine => GL2EngineProvider.Instance;

        public IRendererProvider Renderers => GL2RendererProvider.Instance;

        public IFramebufferProvider Framebuffers => GLFramebufferProvider.Instance;

        public IShaderProvider Shaders => GL2ShaderProvider.Instance;

        public ITextureProvider Textures => GLTextureProvider.Instance;
        #endregion
    }
}
