using OpenTK.Graphics.OpenGL;

using ThinGin.OpenGL.Common.RenderHardware.Resources.Samplers;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Textures
{
    internal class OpenGLTextureContext
    {
        #region Values
        private TextureTarget target;
        private FramebufferAttachment attachment = (FramebufferAttachment)(-1);
        private OpenGLSamplerState samplerState;
        private RHIOpenGLResourceHandle srvResource;
        #endregion

        #region Properties
        /// <summary> MSAA tiled sampling number </summary>
        public readonly uint NumSampleTileMem;
        #endregion

        #region Accessors
        public TextureTarget Target { get => target; set => target = value; }
        public FramebufferAttachment Attachment { get => attachment; set => attachment = value; }
        public OpenGLSamplerState SamplerState { get => samplerState; set => samplerState = value; }
        public RHIOpenGLResourceHandle SRVResource { get => srvResource; set => srvResource = value; }
        #endregion

        #region Constructors
        public OpenGLTextureContext(TextureTarget target, uint numSampleTileMem)
        {
            NumSampleTileMem = numSampleTileMem;
            this.target = target;
        }

        public OpenGLTextureContext(TextureTarget target, uint numSampleTileMem, FramebufferAttachment attachment)
        {
            NumSampleTileMem = numSampleTileMem;
            this.target = target;
            this.attachment = attachment;
        }

        #endregion

    }
}
