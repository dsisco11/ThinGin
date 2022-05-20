using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTexture1D : RHITexture1D, IOpenGLTexture
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
        public OpenGLTexture1D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint NumMips, in uint NumSampleTileMem) : base(rhi, Format, Flags, SizeX, NumMips)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture1D, NumSampleTileMem);
        }

        public OpenGLTexture1D(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint NumMips, in uint NumSamples, in uint NumSampleTileMem, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, NumMips, NumSamples, ClearValue)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture1D, NumSampleTileMem);
        }
        #endregion
    }
}
