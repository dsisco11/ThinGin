using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;

namespace ThinGin.Core.Common.Providers
{
    public interface ITextureProvider
    {
        Texture Create(IEngine Engine, PixelDescriptor GpuLayout);
        TextureHandle Create_Handle(IEngine Engine);
    }
}
