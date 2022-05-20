using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Textures;
using ThinGin.Core.Engine.Common.Core;
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
        public GLTexture2DArray(EngineInstance engine, PixelDescriptor GpuLayout) : base(engine, GpuLayout)
        {
            if (!engine.Renderer.IsSupported("ext_texture_array"))
            {
                throw new OpenGLUnsupportedException(nameof(GLTexture2DArray));
            }
        }
        #endregion

        #region Uploading
        protected override void Upload(byte[] RawPixels, int miplevel)
        {
            var engine = RHI as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(HardwareLayout);
            var pxlTyp = engine.Get_PixelType(HardwareLayout);
            var datFmt = engine.Get_Internal_PixelFormat(Metadata.Layout, UseCompression);

            if (RawPixels is object)
            {
                GL.TexImage3D(Target, miplevel, datFmt, Metadata.SizeX, Metadata.SizeY, Metadata.SizeZ, 0, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexImage3D(Target, miplevel, datFmt, Metadata.SizeX, Metadata.SizeY, Metadata.SizeZ, 0, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }

        protected override void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel)
        {
            var engine = RHI as GLEngineBase;
            var pxlFmt = engine.Get_PixelFormat(HardwareLayout);
            var pxlTyp = engine.Get_PixelType(HardwareLayout);

            if (RawPixels is object)
            {
                GL.TexSubImage3D(Target, miplevel, xoffset, yoffset, zoffset, Metadata.SizeX, Metadata.SizeY, Metadata.SizeZ, pxlFmt, pxlTyp, RawPixels);
            }
            else
            {
                GL.TexSubImage3D(Target, miplevel, xoffset, yoffset, zoffset, Metadata.SizeX, Metadata.SizeY, Metadata.SizeZ, pxlFmt, pxlTyp, IntPtr.Zero);
            }
        }
        #endregion
    }
}
