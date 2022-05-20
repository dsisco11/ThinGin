using OpenTK.Graphics.OpenGL;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Samplers
{
    public readonly struct OpenGLSamplerStateData
    {
        #region Values
        public readonly TextureMinFilter MinFilter;
        public readonly TextureMagFilter MagFilter;
        public readonly TextureWrapMode WrapR;
        public readonly TextureWrapMode WrapS;
        public readonly TextureWrapMode WrapT;

        /// <summary>
        /// Valid values are: <seealso cref="All.Always"/>, <seealso cref="All.Never"/>, <seealso cref="All.Less"/>, <seealso cref="All.Lequal"/>, <seealso cref="All.Equal"/>, <seealso cref="All.Gequal"/>, <seealso cref="All.Greater"/>, <seealso cref="All.Lequal"/>
        /// </summary>
        public readonly All Func;
        public readonly TextureCompareMode Mode;
        public readonly uint LODBias;
        public readonly uint MaxAnisotropy;
        #endregion

        #region Constructors
        #endregion

    }
}
