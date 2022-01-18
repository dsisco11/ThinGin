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
    public class GLTexture1DArray : GLTexture
    {
        #region Accessors
        public override EnableCap Encap => EnableCap.Texture1D;
        public override TextureTarget Target => TextureTarget.Texture1DArray;
        #endregion

        #region Constructors
        public GLTexture1DArray(IRenderEngine Engine, PixelDescriptor GpuLayout) : base(Engine, GpuLayout)
        {
            if (!Engine.IsSupported("ext_texture_array"))
            {
                throw new OpenGLUnsupportedException(nameof(GLTexture1DArray));
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
                GL.TexImage2D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, 0, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexImage2D(Target, miplevel, datFmt, Metadata.Width, Metadata.Height, 0, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }

        protected override void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel)
        {
            var engine = Engine as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(GpuLayout);
            var pxlTyp = engine.Get_PixelType(GpuLayout);

            if (RawPixels is object)
            {
                GL.TexSubImage2D(Target, miplevel, xoffset, yoffset, Metadata.Width, Metadata.Height, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexSubImage2D(Target, miplevel, xoffset, yoffset, Metadata.Width, Metadata.Height, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }
        #endregion
    }
}
