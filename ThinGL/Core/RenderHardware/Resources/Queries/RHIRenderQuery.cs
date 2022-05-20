namespace ThinGin.Core.RenderHardware.Resources.Queries
{
    public abstract class RHIRenderQuery : RHIResource
    {
        #region Values
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIRenderQuery(IRHI rhi) : base(rhi)
        {
        }
        #endregion

        #region Lifespan
        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
        #endregion
    }
}
