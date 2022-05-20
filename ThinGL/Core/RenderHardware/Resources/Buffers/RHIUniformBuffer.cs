namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIUniformBuffer : RHIRamResource
    {
        #region Values
        public readonly uint Size;
        public readonly bool IsGlobal;
        public readonly RHIUniformBufferLayout Layout;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIUniformBuffer(in IRHI rhi, in uint Size, in bool IsGlobal) : base(rhi)
        {
            this.Size = Size;
            this.IsGlobal = IsGlobal;
        }
        #endregion


        public abstract void Swap(in RHIUniformBuffer other);
    }
}
