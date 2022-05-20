namespace ThinGin.Core.RenderHardware.Resources.Rasterizer
{
    public abstract class RHIRasterizerState : RHIUntrackedResource
    {
        #region Values
        #endregion

        #region Constructors
        public RHIRasterizerState(in IRHI rhi, in RasterizerStateConfig Config) : base(rhi)
        {
        }
        #endregion

    }
}
