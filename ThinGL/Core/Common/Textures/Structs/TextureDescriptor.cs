using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Textures
{
    /// <summary>
    /// Describes a textures data
    /// </summary>
    public struct TextureDescriptor
    {
        #region Properties
        /// <summary> The size, in pixels, of the textures X axis </summary>
        public readonly uint SizeX;
        /// <summary> The size, in pixels, of the textures Y axis </summary>
        public readonly uint SizeY;
        /// <summary> The size, in pixels, of the textures Z axis </summary>
        public readonly uint SizeZ;
        /// <summary> The size, in pixels, of the textures W axis </summary>
        public readonly uint SizeW;
        /// <summary> The number of mipmaps which the texture has. </summary>
        public readonly uint NumMips;
        public readonly EPixelFormat Format;
        #endregion

        #region Constructors
        public TextureDescriptor(EPixelFormat pixelFormat, uint numMipmaps=0, uint sizeX=0, uint sizeY=0, uint sizeZ=0, uint sizeW=0)
        {
            Format = pixelFormat;
            NumMips = numMipmaps;
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            SizeW = sizeW;
        }
        #endregion
    }
}
