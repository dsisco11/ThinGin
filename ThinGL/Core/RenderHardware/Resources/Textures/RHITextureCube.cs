using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITextureCube : RHITexture
    {
        #region Values
        public readonly uint Size;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHITextureCube(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint Size, in uint NumMips) : base(rhi, Format, Flags, NumMips)
        {
            this.Size = Size;
        }

        public RHITextureCube(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint Size, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, NumMips, ClearValue)
        {
            this.Size = Size;
        }
        #endregion

        #region Casts
        public override RHITextureCube AsTextureCube() => this as RHITextureCube;
        #endregion
    }
}
