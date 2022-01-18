using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.OpenGL.Common.Providers;
using ThinGin.OpenGL.GL3.Providers;

namespace ThinGin.OpenGL.GL3
{
    public sealed class Implementation : IGraphicsImplementation
    {
        #region Static Instance
        public static IGraphicsImplementation Instance = new Implementation();
        #endregion

        #region Providers
        public IEngineProvider Engine => GL3EngineProvider.Instance;

        public IRendererProvider Renderers => GL3RendererProvider.Instance;

        public IFramebufferProvider Framebuffers => GLFramebufferProvider.Instance;

        public IShaderProvider Shaders => GL3ShaderProvider.Instance;

        public ITextureProvider Textures => GLTextureProvider.Instance;
        #endregion
    }
}
