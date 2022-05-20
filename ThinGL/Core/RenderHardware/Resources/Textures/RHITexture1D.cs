using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture1D : RHITexture
    {
        #region Values
        public readonly uint SizeX;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITexture1D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint NumMips) : base(rhi, Format, Flags, NumMips)
        {
            this.SizeX = SizeX;
        }
        
        public RHITexture1D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint NumMips, in uint NumSamples, in ClearValue ClearValue) : base(rhi, Format, Flags, NumMips, ClearValue, NumSamples)
        {
            this.SizeX = SizeX;
        }
        #endregion

        #region Casting
        public override RHITexture1D AsTexture1D() => this as RHITexture1D;
        #endregion
    }
}
