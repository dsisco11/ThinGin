using ThinGin.Core.Common.Engine;

namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// A managed resource is one which is tracked and monitored over time and which may be unloaded from either CPU or GPU memory once it is no longer needed.
    /// </summary>
    public class RHIManagedResource : RHIResource
    {
        #region Values
        public readonly RenderTimestamp LastRenderTime = null;

        /// <summary>
        /// A textures priority is a number indicating its importance, eg; a number which indicates its unload order when clearing memory for resource management. Resources are unloaded in order according to their priority, with lowest priority numbers being first.
        /// <para>Higher values indicate lower chance of being forced to unload in resource constrained scenarios.</para>
        /// </summary>
        /// <returns>Integer representing position in the unloading order</returns>
        public int ResourcePriority { get; set; } = 0;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIManagedResource(IRHI rhi, RenderTimestamp last_render = null) : base(rhi)
        {
            LastRenderTime = last_render;
        }
        #endregion

        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
    }
}
