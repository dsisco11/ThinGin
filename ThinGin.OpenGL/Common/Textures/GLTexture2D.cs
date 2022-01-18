using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTexture2D : GLTexture
    {
        #region Accessors
        public override EnableCap Encap => EnableCap.Texture2D;
        public override TextureTarget Target => TextureTarget.Texture2D;
        #endregion

        #region Constructors
        public GLTexture2D(IRenderEngine Engine, PixelDescriptor GpuLayout) : base(Engine, GpuLayout)
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

            //GL.BindTexture(Target, _id);
            if (RawPixels is object)
            {
                GL.TexImage2D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, 0, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexImage2D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, 0, pxlFmt, pxlTyp, IntPtr.Zero);
            }
            //GL.BindTexture(Target, 0);
        }

        protected override void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel)
        {
            var engine = Engine as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(GpuLayout);
            var pxlTyp = engine.Get_PixelType(GpuLayout);

            //GL.BindTexture(Target, _id);
            if (RawPixels is object)
            {
                GL.TexSubImage2D(Target, miplevel, xoffset, yoffset, Metadata.Width, Metadata.Height, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexSubImage2D(Target, miplevel, xoffset, yoffset, Metadata.Width, Metadata.Height, pxlFmt, pxlTyp, IntPtr.Zero);
            }
            //GL.BindTexture(Target, 0);
        }
        #endregion
    }
}
