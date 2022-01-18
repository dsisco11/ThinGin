namespace ThinGin.Core.Common.Textures
{
    public struct TextureMetadata
    {
        #region Properties
        public readonly PixelDescriptor Layout;
        public readonly int Width;
        public readonly int Height;
        public readonly int Depth;
        #endregion

        #region Constructors
        public TextureMetadata(PixelDescriptor pixelFormat, int width=1, int height=1, int depth=1)
        {
            Layout = pixelFormat;
            Width = width;
            Height = height;
            Depth = depth;
        }
        #endregion
    }
}
