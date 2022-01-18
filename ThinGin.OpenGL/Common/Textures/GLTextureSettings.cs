
using OpenTK.Graphics.OpenGL;

namespace ThinGin.OpenGL.Common.Textures
{
    public struct GLTextureSettings
    {
        #region Statics
        public static GLTextureSettings Default = new GLTextureSettings(true);
        public static GLTextureSettings NoWrap = new GLTextureSettings(AllowWrapping: false);
        #endregion

        #region Values
        public int Type;
        public int Priority;
        public bool GenerateMipMaps;

        public TextureWrapMode WrapS;
        public TextureWrapMode WrapT;

        public TextureMinFilter MinFilter;
        public TextureMagFilter MagFilter;

        public TextureEnvMode EnvMode;
        #endregion

        #region Constructors
        public GLTextureSettings(bool AllowWrapping) : this(Type: 0, Priority: 0, AllowWrapping: AllowWrapping)
        {
        }

        public GLTextureSettings(int Type = 0, int Priority = 0, bool GenerateMipMaps = true, bool AllowWrapping = true, TextureMinFilter? minFilter = null, TextureMagFilter? magFilter = null)
        {
            this.Type = Type;
            this.Priority = Priority;
            this.GenerateMipMaps = GenerateMipMaps;

            WrapS = AllowWrapping ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
            WrapT = AllowWrapping ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;

            //this.MinFilter = TextureMinFilter.LinearClipmapLinearSgix;
            //this.MagFilter = TextureMagFilter.LinearDetailSgis;

            MinFilter = GenerateMipMaps ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear;
            MagFilter = TextureMagFilter.Linear;

            // If we have specified min/mag filtering values then override the defaults
            if (minFilter.HasValue) MinFilter = minFilter.Value;
            if (magFilter.HasValue) MagFilter = magFilter.Value;

            //this.MinFilter = TextureMinFilter.Linear;
            //this.MagFilter = TextureMagFilter.Linear;

            EnvMode = TextureEnvMode.Modulate;// XXX: Try blend
        }
        #endregion
    }
}
