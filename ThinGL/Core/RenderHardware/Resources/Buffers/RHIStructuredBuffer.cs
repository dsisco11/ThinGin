using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIStructuredBuffer : RHIRamResource
    {
        #region Values
        public readonly uint Size;
        public readonly uint Stride;
        public readonly EBufferUpdate Usage;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIStructuredBuffer(in IRHI rhi, in uint Size, in uint Stride, in EBufferUpdate Usage) : base(rhi)
        {
            this.Size = Size;
            this.Stride = Stride;
            this.Usage = Usage;
        }
        #endregion

    }
}
