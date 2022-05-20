using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware.Synchronization
{
    public abstract class RHIComputeFence : RHIUntrackedResource
    {
        protected RHIComputeFence(in IRHI rhi) : base(rhi)
        {
        }
    }
}
