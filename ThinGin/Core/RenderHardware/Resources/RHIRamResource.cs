using ThinGin.Core.Common.Engine;

namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Represents a resource which takes up space in VRAM
    /// </summary>
    public abstract class RHIRamResource : RHIManagedResource
    {
        #region Values
        private RHIResourceInfo resourceinfo;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        /// <summary>
        /// Details the resources hardware memory usage
        /// </summary>
        public RHIResourceInfo ResourceInfo => resourceinfo;
        #endregion

        #region Constructors
        public RHIRamResource(IRHI rhi, RenderTimestamp last_render = null) : base(rhi, last_render)
        {
        }
        #endregion

        internal void Set_ResourceInfo(RHIResourceInfo info)
        {
            resourceinfo = info;
        }
    }
}
