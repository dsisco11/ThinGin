namespace ThinGin.Core.RenderHardware.Synchronization
{
    public class RHIGenericFenceObject : RHIFenceObject
    {
        #region Values
        private readonly uint StartingFrame;
        #endregion

        #region Constructors
        public RHIGenericFenceObject(RenderHardwareInterface rhi) : base(rhi)
        {
            StartingFrame = rhi.CurrentFrame;
        }
        #endregion

        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;

        protected override bool Poll()
        {
            return (StartingFrame < RHI.CurrentFrame);
        }
    }
}
