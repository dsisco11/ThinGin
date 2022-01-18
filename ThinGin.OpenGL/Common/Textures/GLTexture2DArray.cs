using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;
using ThinGin.OpenGL.Common.Exceptions;

namespace ThinGin.OpenGL.Common.Textures
{
    /// <summary>
    /// "An Array Texture is a Texture where each mipmap level contains an array of images of the same size. 
    /// Array textures may have Mipmaps, but each mipmap in the texture has the same number of levels."
    /// <para>See: https://www.khronos.org/opengl/wiki/Array_Texture</para>
    /// </summary>
    public class GLTexture2DArray : GLTexture
    {
        #region Accessors
        public override EnableCap Encap => EnableCap.Texture2D;
        public override TextureTarget Target => TextureTarget.Texture2DArray;
        #endregion

        #region Constructors
        public GLTexture2DArray(IRenderEngine Engine, PixelDescriptor GpuLayout) : base(Engine, GpuLayout)
        {
            if (!Engine.IsSupported("ext_texture_array"))
            {
                throw new OpenGLUnsupportedException(nameof(GLTexture2DArray));
            }
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
