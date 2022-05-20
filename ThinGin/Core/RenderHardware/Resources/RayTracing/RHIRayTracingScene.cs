namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Top level ray tracing acceleration structure, which contains mesh instances.
    /// </summary>
    public abstract class RHIRayTracingScene : RHIResource
    {
        #region Values
        private RHIShaderResourceView resources;
        #endregion

        #region Accessors
        public RHIShaderResourceView Resources { get => resources; set => resources = value; }
        #endregion

        #region Constructors
        protected RHIRayTracingScene(IRHI rhi) : base(rhi)
        {
        }
        #endregion
    }
}
