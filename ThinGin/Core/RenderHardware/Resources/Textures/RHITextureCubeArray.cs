using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITextureCubeArray : RHITextureCube
    {
        #region Values
        public readonly uint ArraySize;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITextureCubeArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint Size, in uint ArraySize, in uint NumMips) : base(rhi, Format, Flags, Size, NumMips)
        {
            this.ArraySize = ArraySize;
        }

        public RHITextureCubeArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint Size, in uint ArraySize, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, Size, NumMips, ClearValue)
        {
            this.ArraySize = ArraySize;
        }
        #endregion

        #region Casts
        public override RHITextureCubeArray AsTextureCubeArray() => this as RHITextureCubeArray;
        #endregion
    }
}
