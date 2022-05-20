using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture1DArray : RHITexture1D
    {
        #region Values
        public readonly uint ArraySize;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITexture1DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint ArraySize, in uint NumMips) : base(rhi, Format, Flags, SizeX, NumMips)
        {
            this.ArraySize = ArraySize;
        }
        
        public RHITexture1DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint ArraySize, in uint NumMips, in uint NumSamples, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, NumMips, NumSamples, ClearValue)
        {
            this.ArraySize = ArraySize;
        }
        #endregion

        #region Casting
        public override RHITexture1DArray AsTexture1DArray() => this as RHITexture1DArray;
        #endregion
    }
}
