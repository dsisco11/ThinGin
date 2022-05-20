using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture2D : RHITexture
    {
        #region Values
        public readonly uint SizeX;
        public readonly uint SizeY;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITexture2D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint NumMips) : base(rhi, Format, Flags, NumMips)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }
        
        public RHITexture2D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint NumMips, in uint NumSamples, in ClearValue ClearValue) : base(rhi, Format, Flags, NumMips, ClearValue, NumSamples)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }
        #endregion

        #region Casting
        public override RHITexture2D AsTexture2D() => this as RHITexture2D;
        #endregion
    }
}
