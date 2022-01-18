using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.OpenGL.Common.Textures;

namespace ThinGin.OpenGL.Common.Providers
{
    public class GLTextureProvider : ITextureProvider
    {
        #region Static Properties
        public static ITextureProvider Instance = new GLTextureProvider();
        #endregion


        public Texture Create(IRenderEngine Engine, PixelDescriptor GpuLayout)
        {
            return new GLTexture2D(Engine, GpuLayout);
        }

        public TextureHandle Create_Handle(IRenderEngine Engine)
        {
            return new GLTextureHandle(Engine);
        }
    }
}
