namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// An RHI texture proxy is a wrapper around another texture resource, 
    /// it acts as a proxy to said texture and can have different properties from its source in order to alter the way the RHI accesses said texture during rendering.
    /// <note>This has nothing to do with the similarly named OpenGL concept!.</note>
    /// </summary>
    public abstract class RHITextureProxy : RHITexture
    {
        #region Values
        private RHITexture source;
        #endregion

        #region Accessors
        public RHITexture Source => source;
        #endregion

        #region Constructors
        public RHITextureProxy(in IRHI rhi) : base(rhi)
        {
        }
        #endregion

        /// <summary>
        /// Sets the source texture of the proxy
        /// </summary>
        /// <param name="texture"></param>
        public void SetProxySource(in RHITexture texture)
        {
            source = texture;
        }
    }
}
