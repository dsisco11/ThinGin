using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTexture2DMultisampleArray : GLTextureMultisample
    {
        #region Properties
        #endregion

        #region Accessors
        public override EnableCap Encap => EnableCap.Texture3DExt;
        public override TextureTarget Target => TextureTarget.Texture2DMultisampleArray;
        #endregion

        #region Constructors
        public GLTexture2DMultisampleArray(IEngine Engine, PixelDescriptor GpuLayout, int Samples) : base(Engine, GpuLayout, Samples)
        {
        }
        #endregion

        #region Uploading
        protected override void Upload(byte[] RawPixels, int miplevel)
        {
            var engine = Engine as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(GpuLayout);
            var pxlTyp = engine.Get_PixelType(GpuLayout);
            var datFmt = engine.Get_Internal_PixelFormat(Metadata.Layout, UseCompression);

            GL.TexImage3DMultisample(TextureTargetMultisample.Texture2DMultisample, Samples, datFmt, Metadata.Width, Metadata.Height, Metadata.Depth, false);
        }
        #endregion
    }
}
