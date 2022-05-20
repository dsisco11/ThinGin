using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture3D : RHITexture
    {
        #region Values
        public readonly uint SizeX;
        public readonly uint SizeY;
        public readonly uint SizeZ;
        #endregion

        #region Accessors
        #endregion


        #region Constructors
        public RHITexture3D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips) : base(rhi, Format, Flags, NumMips)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
        }

        public RHITexture3D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, NumMips, ClearValue)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
        }
        #endregion

        #region Casting
        public override RHITexture3D AsTexture3D() => this as RHITexture3D;
        #endregion
    }
}
