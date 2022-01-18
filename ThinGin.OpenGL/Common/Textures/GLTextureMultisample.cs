using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;
using ThinGin.OpenGL.Common.Exceptions;

namespace ThinGin.OpenGL.Common.Textures
{
    public abstract class GLTextureMultisample : GLTexture
    {
        #region Properties
        public readonly int Samples;
        #endregion

        #region Constructors
        public GLTextureMultisample(IRenderEngine Engine, PixelDescriptor GpuLayout, int samples) : base(Engine, GpuLayout)
        {
            if (!Engine.IsSupported("arb_texture_multisample"))
            {
                throw new OpenGLUnsupportedException(nameof(GLTextureMultisample));
            }

            Samples = samples;
        }
        #endregion


        #region Initialization
        public override bool TryLoad(TextureMetadata Metadata, byte[] RawPixels)
        {
            try
            {
                this.Metadata = Metadata;
                _id = GL.GenTexture();

                Set_Params();
                Upload(null, 0);
            }
            catch
            {
                return false;
            }
            finally
            {
                RawPixels = null;
            }

            return true;
        }
        #endregion

        #region Uploading
        protected override void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
