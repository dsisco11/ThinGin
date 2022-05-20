using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTexture2DArray : RHITexture2DArray, IOpenGLTexture
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
        public OpenGLTexture2DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint NumMips, in uint NumSampleTileMem, in uint ArraySize) : base(rhi, Format, Flags, SizeX, SizeY, ArraySize, NumMips)
        {
            context = new OpenGLTextureContext(
                target: (NumSamples > 1) ? TextureTarget.Texture2DMultisampleArray : TextureTarget.Texture2DArray, 
                NumSampleTileMem);
        }

        public OpenGLTexture2DArray(in IRHI rhi, in EPixelFormat Format, in ETextureCreationFlags Flags, in uint SizeX, in uint SizeY, in uint NumMips, in uint NumSamples, in uint NumSampleTileMem, in uint ArraySize, in ClearValue ClearValue) : base(rhi, Format, Flags, SizeX, SizeY, ArraySize, NumMips, NumSamples, ClearValue)
        {
            context = new OpenGLTextureContext(
                target: (NumSamples > 1) ? TextureTarget.Texture2DMultisampleArray : TextureTarget.Texture2DArray,
                NumSampleTileMem);
        }
        #endregion
    }
}
