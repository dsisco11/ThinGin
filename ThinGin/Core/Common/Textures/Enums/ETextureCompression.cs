namespace ThinGin.Core.Common.Textures
{
    public enum ETextureCompression
    {
        /// <summary> DXT1 without alpha </summary>
        DXT1,
        /// <summary> DXT1 with alpha </summary>
        DXT1A,
        /// <summary>  </summary>
        DXT3,
        /// <summary>  </summary>
        DXT5,

        VU,
        UYVY,

        /// <summary> BPTC compression </summary>
        BC4,
        /// <summary> BPTC compression </summary>
        BC5,
        /// <summary> BPTC compression </summary>
        BC6H,
        /// <summary> BPTC compression </summary>
        BC7,

        ASTC_4,
        ASTC_6,
        ASTC_8,
        ASTC_10,
        ASTC_12,

    }
}
