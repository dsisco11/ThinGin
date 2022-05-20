using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTextureCubeArray : RHITextureCubeArray, IOpenGLTexture
    {
        #region Values
        private readonly OpenGLTextureContext context;
        #endregion

        #region Properties
        public readonly uint SizeX;
        public readonly uint SizeY;
        public readonly uint SizeZ;
        #endregion

        #region Accessors
        public OpenGLTextureContext Context => context;
        #endregion

        #region Constructors
        public OpenGLTextureCubeArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint ArraySize, in uint NumMips) : base(rhi, Format, Flags, SizeX, ArraySize, NumMips)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
            context = new OpenGLTextureContext(TextureTarget.TextureCubeMapArray, 0);
        }

        public OpenGLTextureCubeArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint ArraySize, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, ArraySize, NumMips, ClearValue)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
            context = new OpenGLTextureContext(TextureTarget.TextureCubeMapArray, 0);
        }
        #endregion
    }
}
