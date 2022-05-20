using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTexture2DMultisample : GLTextureMultisample
    {
        #region Properties
        #endregion

        #region Accessors
        public override EnableCap Encap => EnableCap.Texture2D;
        public override TextureTarget Target => TextureTarget.Texture2DMultisample;
        #endregion

        #region Constructors
        public GLTexture2DMultisample(IEngine Engine, PixelDescriptor GpuLayout, int Samples) : base(Engine, GpuLayout, Samples)
        {
        }
        #endregion

        #region Uploading
        protected override void Upload(byte[] RawPixels, int miplevel)
        {
            var engine = RHI as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(HardwareLayout);
            var pxlTyp = engine.Get_PixelType(HardwareLayout);
            var datFmt = engine.Get_Internal_PixelFormat(Metadata.Layout, UseCompression);

            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, Samples, datFmt, Metadata.SizeX, Metadata.SizeY, false);
        }
        #endregion
    }
}
