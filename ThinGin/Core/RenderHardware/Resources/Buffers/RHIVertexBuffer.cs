using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIVertexBuffer : RHIRamResource
    {
        #region Values
        public readonly uint Size;
        public readonly EBufferUpdate Usage;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIVertexBuffer(in IRHI rhi, in uint Size, in EBufferUpdate Usage) : base(rhi)
        {
            this.Size = Size;
            this.Usage = Usage;
        }
        #endregion


        public abstract void Swap(in RHIVertexBuffer other);
    }
}
