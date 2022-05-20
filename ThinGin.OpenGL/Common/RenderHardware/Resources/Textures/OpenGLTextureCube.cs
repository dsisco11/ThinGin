using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTextureCube : RHITextureCube, IOpenGLTexture
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
        public OpenGLTextureCube(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips) : base(rhi, Format, Flags, SizeX, NumMips)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
            context = new OpenGLTextureContext(TextureTarget.TextureCubeMap, 0);
        }

        public OpenGLTextureCube(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, NumMips, ClearValue)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;
            this.SizeZ = SizeZ;
            context = new OpenGLTextureContext(TextureTarget.TextureCubeMap, 0);
        }
        #endregion
    }
}
