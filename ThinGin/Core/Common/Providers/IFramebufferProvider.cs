using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Rendering;

namespace ThinGin.Core.Common.Providers
{
    public interface IFramebufferProvider
    {
        GBuffer Create_FrameBuffer(IEngine Engine, System.Drawing.Size Size);
    }
}
