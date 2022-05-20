using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware
{
    public interface IRHI
    {
        uint CurrentFrame { get; }
        RHIResourceManager Resources { get; }

        void Shutdown();
    }
}