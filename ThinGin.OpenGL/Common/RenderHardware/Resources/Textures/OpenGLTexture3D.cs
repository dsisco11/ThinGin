using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTexture3D : RHITexture3D, IOpenGLTexture
    {
        #region Values
        private readonly OpenGLTextureContext context;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public OpenGLTextureContext Context => context;
        #endregion

        #region Constructors
        public OpenGLTexture3D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips) : base(rhi, Format, Flags, SizeX, SizeY, SizeZ, NumMips)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture3D, 1);
        }

        public OpenGLTexture3D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint SizeZ, in uint NumMips, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, SizeY, SizeZ, NumMips, ClearValue)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture3D, 1);
        }
        #endregion
    }
}
