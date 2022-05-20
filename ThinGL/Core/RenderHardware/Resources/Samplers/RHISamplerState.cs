namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Subclassed by implementations in order to store a bunch of state/settings for a texture sampler like wrapping modes, etc
    /// </summary>
    public abstract class RHISamplerState : RHIResource
    {
        #region Values
        // Note: we might actually need fence objects here for synchronizing with the render thread
        #endregion

        #region Properties
        public abstract bool IsImmutable { get; }
        #endregion

        #region Constructors
        public RHISamplerState(in IRHI rhi) : base(rhi)
        {
        }
        #endregion

        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
        public override RHIDelegate Get_Updater() => null;
    }
}
