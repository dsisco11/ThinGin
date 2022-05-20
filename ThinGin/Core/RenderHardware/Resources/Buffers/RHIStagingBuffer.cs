namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Generic staging buffer used for GPU memory readback, RHI specific staging buffers subclass this
    /// </summary>
    public abstract class RHIStagingBuffer : RHIRamResource
    {
        #region Values
        #endregion

        #region Constructors
        public RHIStagingBuffer(in IRHI rhi) : base(rhi)
        {
        }
        #endregion

    }
}
