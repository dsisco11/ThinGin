namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Provides a null texture proxy
    /// </summary>
    public class RHITextureProxyNull : RHITextureProxy
    {
        #region Values
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITextureProxyNull() : base(null)
        {
        }
        #endregion

        public void SetProxySource(in RHITexture texture)
        {
        }
    }
}
