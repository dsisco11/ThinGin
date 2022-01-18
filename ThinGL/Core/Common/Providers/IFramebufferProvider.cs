using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Rendering;

namespace ThinGin.Core.Common.Providers
{
    public interface IFramebufferProvider
    {
        FrameBuffer Create_FrameBuffer(IRenderEngine Engine, System.Drawing.Size Size);
    }
}
