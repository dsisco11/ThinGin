namespace ThinGin.Core.Common.Font
{
    public interface IFontProvider
    {
        IFontRasterizer Create(EFontFamily FontFamily, EFontStyle FontStyle);
        IFontRasterizer Create(string FontName, EFontStyle FontStyle);
    }
}
