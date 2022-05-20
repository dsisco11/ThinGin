using ThinGin.Core.Common.Providers;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IGraphicsImplementation
    {
        IEngineProvider Engine { get; }
        IRendererProvider Renderers { get; }
        IFramebufferProvider Framebuffers { get; }
        IShaderProvider Shaders { get; }
        ITextureProvider Textures { get; }
    }
}
