using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture2DArray : RHITexture2D
    {
        #region Values
        public readonly uint ArraySize;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITexture2DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint ArraySize, in uint NumMips) : base(rhi, Format, Flags, SizeX, SizeY, NumMips)
        {
            this.ArraySize = ArraySize;
        }

        public RHITexture2DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint ArraySize, in uint NumMips, in uint NumSamples, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, SizeY, NumMips, NumSamples, ClearValue)
        {
            this.ArraySize = ArraySize;
        }
        #endregion

        #region Casting
        public override RHITexture2D AsTexture2D() => this as RHITexture2D;
        public override RHITexture2DArray AsTexture2DArray() => this as RHITexture2DArray;
        #endregion
    }
}
