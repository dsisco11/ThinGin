using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTexture1DArray : RHITexture1DArray, IOpenGLTexture
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
        public OpenGLTexture1DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint ArraySize, in uint NumMips, in uint NumSampleTileMem) : base(rhi, Format, Flags, SizeX, ArraySize, NumMips)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture1DArray, NumSampleTileMem);
        }

        public OpenGLTexture1DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint ArraySize, in uint NumMips, in uint NumSamples, in uint NumSampleTileMem, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, ArraySize, NumMips, NumSamples, ClearValue)
        {
            context = new OpenGLTextureContext(TextureTarget.Texture1DArray, NumSampleTileMem);
        }
        #endregion
    }
}
