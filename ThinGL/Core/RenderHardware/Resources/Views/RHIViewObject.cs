namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// The concept of a view object is that it is like a pointer to a hardware data buffer with specified access rights.
    /// All graphics APIs provide equivalent functionality to serve this purpose.
    /// </summary>
    public abstract class RHIViewObject : RHIResource
    {
        #region Values
        private readonly RHIHandle handle = null;
        #endregion

        #region Properties
        public RHIHandle Handle => handle;
        #endregion

        #region Constructors
        public RHIViewObject(in IRHI rhi, in RHIHandle handle) : base(rhi)
        {
            this.handle = handle;
        }
        #endregion

        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
        public override RHIDelegate Get_Updater() => null;

    }
}
