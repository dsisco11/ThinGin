using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.OpenGL.Common.Textures;

namespace ThinGin.OpenGL.Common.Providers
{
    public class GLTextureProvider : ITextureProvider
    {
        #region Static Properties
        public static ITextureProvider Instance = new GLTextureProvider();
        #endregion


        public Texture Create(EngineInstance Engine, PixelDescriptor GpuLayout)
        {
            return new GLTexture2D(Engine, GpuLayout);
        }

        public TextureHandle Create_Handle(EngineInstance Engine)
        {
            return new GLTextureHandle(Engine);
        }
    }
}
