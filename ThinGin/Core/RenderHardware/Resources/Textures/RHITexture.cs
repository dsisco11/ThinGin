using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHITexture : RHIRamResource
    {
        #region Values
        private RHIHandle _native_resource = null;
        #endregion

        #region Properties
        public readonly ClearValue ClearValue = ClearValue.None;
        public readonly ETextureCreationFlags Flags = ETextureCreationFlags.None;
        public readonly EPixelFormat Format = EPixelFormat.PF_Unknown;
        public readonly uint NumMips = 0;
        public readonly uint NumSamples = 0;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        internal RHITexture(in IRHI rhi) : base(rhi)
        {
        }

        public RHITexture(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint NumMips, in uint NumSamples=1) : base(rhi)
        {
            this.Format = Format;
            this.Flags = Flags;
            this.NumMips = NumMips;
            this.NumSamples = NumSamples;
            this.ClearValue = ClearValue.None;
        }

        public RHITexture(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint NumMips, in ClearValue ClearValue, in uint NumSamples=1) : base(rhi)
        {
            this.Format = Format;
            this.Flags = Flags;
            this.NumMips = NumMips;
            this.NumSamples = NumSamples;
            this.ClearValue = ClearValue;
        }
        #endregion

        #region Native Resource
        /// <summary>
        /// Retrieves the native handle to the underlying object implemented by the RHI driver, which this class acts as an abstraction for.
        /// </summary>
        public RHIHandle Get_NativeResource() => _native_resource;

        /// <summary>
        /// Sets the native handle to the underlying object implemented by the RHI driver, which this class acts as an abstraction for.
        /// </summary>
        public void Set_NativeResource(RHIHandle handle) => _native_resource = handle;
        #endregion

        #region Lifespan
        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
        public override RHIDelegate Get_Updater() => null;
        #endregion

        #region Casts
        public virtual RHITexture1D AsTexture1D() => null;
        public virtual RHITexture1DArray AsTexture1DArray() => null;
        public virtual RHITexture2D AsTexture2D() => null;
        public virtual RHITexture2DArray AsTexture2DArray() => null;
        public virtual RHITexture3D AsTexture3D() => null;
        public virtual RHITextureCube AsTextureCube() => null;
        public virtual RHITextureCubeArray AsTextureCubeArray() => null;
        #endregion
    }
}
