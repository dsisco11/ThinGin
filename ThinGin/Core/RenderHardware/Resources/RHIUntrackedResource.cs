namespace ThinGin.Core.RenderHardware.Resources
{
    public class RHIUntrackedResource : RHIResource
    {
        #region Constructors
        protected RHIUntrackedResource(in IRHI rhi) : base(rhi)
        {
        }
        #endregion

        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
    }
}
