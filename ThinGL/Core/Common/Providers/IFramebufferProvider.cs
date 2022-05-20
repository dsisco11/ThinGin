using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Rendering;

namespace ThinGin.Core.Common.Providers
{
    public interface IFramebufferProvider
    {
        GBuffer Create_FrameBuffer(EngineInstance Engine, System.Drawing.Size Size);
    }
}
