using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTexture3D : GLTexture
    {
        #region Accessors
        public override EnableCap Encap => EnableCap.Texture3DExt;
        public override TextureTarget Target => TextureTarget.Texture3D;
        #endregion

        #region Constructors
        public GLTexture3D(IRenderEngine Engine, PixelDescriptor GpuLayout) : base(Engine, GpuLayout)
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

            if (RawPixels is object)
            {
                GL.TexImage3D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, Metadata.Depth, 0, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexImage3D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, Metadata.Depth, 0, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }

        protected override void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel)
        {
            var engine = Engine as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(GpuLayout);
            var pxlTyp = engine.Get_PixelType(GpuLayout);

            if (RawPixels is object)
            {
                GL.TexSubImage3D(Target, miplevel, xoffset, yoffset, zoffset, Metadata.Width, Metadata.Height, Metadata.Depth, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexSubImage3D(Target, miplevel, xoffset, yoffset, zoffset, Metadata.Width, Metadata.Height, Metadata.Depth, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }
        #endregion
    }
}
