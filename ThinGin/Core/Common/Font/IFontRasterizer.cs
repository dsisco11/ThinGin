using System;

using ThinGin.Core.Common.Textures;

namespace ThinGin.Core.Common.Font
{
    /// <summary>
    /// Manages rasterization of text into a bitmap using font information provided at instantiation
    /// </summary>
    public interface IFontRasterizer
    {
        bool TryMeasure(ReadOnlySpan<char> Text, double SizePt, out System.Drawing.RectangleF outTextBounds);

        /// <summary>
        /// Attempts to rasterize text into a bitmap
        /// </summary>
        /// <param name="Text">The series of characters to rasterize</param>
        /// <param name="SizePt">Point size to rasterize at</param>
        /// <param name="outPixelsRaw">The resulting bitmap pixels</param>
        /// <param name="outSize">The resulting size of the bitmap</param>
        /// <param name="outLayout">The resulting pixel layout of the bitmap data</param>
        /// <returns></returns>
        bool TryRasterize(ReadOnlySpan<char> Text, double SizePt, out byte[] outPixelsRaw, out System.Drawing.Size outSize, out PixelDescriptor outLayout);
    }
}
