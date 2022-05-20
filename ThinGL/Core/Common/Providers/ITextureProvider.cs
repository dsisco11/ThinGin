using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Providers
{
    public interface ITextureProvider
    {
        Texture Create(EngineInstance Engine, PixelDescriptor GpuLayout);
        TextureHandle Create_Handle(EngineInstance Engine);
    }
}
